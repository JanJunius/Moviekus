using Microsoft.EntityFrameworkCore;
using Moviekus.Dto;
using Moviekus.Dto.MovieDb;
using Moviekus.EntityFramework;
using Moviekus.Models;
using Moviekus.ServiceContracts;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Moviekus.Services
{
    public class MovieService : BaseService<Movie>, IMovieService
    {
        public Movie CreateMovie()
        {
            Movie movie = Movie.CreateNew<Movie>();

            movie.ReleaseDate = movie.LastSeen = MoviekusDefines.MinDate;

            return movie;
        }

        public async Task<IEnumerable<Movie>> GetWithSourceAsync()
        {
            using (var context = new MoviekusDbContext())
            {
                return await context.Movies.Include(m => m.Source).ToListAsync();               
            }
        }

        public async Task<IEnumerable<Movie>> GetWithGenresAsync()
        {
            using (var context = new MoviekusDbContext())
            {
                return await context.Movies.Include(m => m.MovieGenres).ThenInclude(g => g.Genre).ToListAsync();
            }
        }

        public async Task<IList<Movie>> GetWithGenresAndSourcesAsync(MovieSortOrder sortOrder)
        {
            using (var context = new MoviekusDbContext())
            {
                var movies = await context.Movies.Include(s => s.Source).ToListAsync();

                foreach(var movie in movies)
                {
                    // Dieser Aufruf führt dazu, dass die bereits geladenen Movie-Objekte mit den MovieGenres angereichert werden
                    // Ein manuelles Add ist nicht erforderlich!
                    var movieGenres = context.MovieGenres.Where(mg => mg.Movie == movie).Include(g => g.Genre).ToList();
                }

                switch (sortOrder)
                {
                    case MovieSortOrder.Title:
                        return movies.OrderBy(m => m.Title).ToList();
                    case MovieSortOrder.LastSeen:
                        return movies.OrderBy(m => m.LastSeen).ToList();
                    case MovieSortOrder.Rating:
                        return movies.OrderByDescending(m => m.Rating).ToList();
                    case MovieSortOrder.ReleaseDate:
                        return movies.OrderByDescending(m => m.ReleaseDate).ToList();
                    case MovieSortOrder.Runtime:
                        return movies.OrderBy(m => m.Runtime).ToList();
                    case MovieSortOrder.EpisodeNumber:
                        return movies.OrderBy(m => m.EpisodeNumber).ToList();
                    default: return movies;
                }
            }

            /*
             * Diese Variante funktioniert nur bei kleinen Datenmengen. Danach kommt es zu folgendem, nicht näher erklärbarem Fehler:
             * SQLite Error 1: 'SQL logic error' 
            using (var context = new MoviekusDbContext())
            {
                switch(sortOrder)
                {
                    case MovieSortOrder.Title:
                        return await context.Movies.OrderBy(m => m.Title).Include(s => s.Source).Include(m => m.MovieGenres).ThenInclude(g => g.Genre).ToListAsync();
                    case MovieSortOrder.LastSeen:
                        return await context.Movies.OrderBy(m => m.LastSeen).Include(s => s.Source).Include(m => m.MovieGenres).ThenInclude(g => g.Genre).ToListAsync();
                    case MovieSortOrder.Rating:
                        return await context.Movies.OrderBy(m => m.Rating).Include(s => s.Source).Include(m => m.MovieGenres).ThenInclude(g => g.Genre).ToListAsync();
                    case MovieSortOrder.ReleaseDate:
                        return await context.Movies.OrderBy(m => m.ReleaseDate).Include(s => s.Source).Include(m => m.MovieGenres).ThenInclude(g => g.Genre).ToListAsync();
                    case MovieSortOrder.Runtime:
                        return await context.Movies.OrderBy(m => m.Runtime).Include(s => s.Source).Include(m => m.MovieGenres).ThenInclude(g => g.Genre).ToListAsync();
                    default:
                        return await context.Movies
                            .Include(s => s.Source)
                            .Include(m => m.MovieGenres)
                            .ThenInclude(g => g.Genre)
                            .ToListAsync();
                }               
            }
            */
        }

        public async Task<Movie> GetWithGenresAndSourcesAsync(string id)
        {
            using (var context = new MoviekusDbContext())
            {
                Movie movie = await context.Movies.Include(s => s.Source).Where(m => m.Id == id).FirstOrDefaultAsync();

                // Dieser Aufruf führt dazu, dass Movie mit den MovieGenres angereichert wird
                // Ein manuelles Add ist nicht erforderlich!
                var movieGenres = context.MovieGenres.Where(mg => mg.Movie == movie).Include(g => g.Genre).ToList();

                return movie;
            }
        }

        public MovieDetailDto GetMovieDetails(Movie movie)
        {
            return new MovieDetailDto() { Movie = movie };
        }

        public async Task<Movie> SaveMovieAsync(Movie movie)
        {
            await SaveChangesAsync(movie);
            return movie;
        }

        /// <summary>
        /// Gleicht die Liste der Genres für den Film miot der übergebenen Id mit den Genres ab, deren
        /// Ids mitgeliefert werden. Neue Genres werden hinzugefügt, entfernte werden auf Deleted gesetzt.
        /// </summary>
        /// <param name="movieId">Id des Films, dessen Genres abgeglichen werdem sollem</param>
        /// <param name="genreIds">Liste der Ids aller Genres, die dem Film zugeordnet werden sollen</param>
        /// <returns>Alle MovieGenres für den Film (die gelöschten auch mit IsDeleted==true)</returns>
        public async Task<ICollection<MovieGenre>> SyncMovieGenres(string movieId, IList<string> genreIds)
        {
            List<MovieGenre> movieGenres = new List<MovieGenre>();

            var movie = await GetWithGenresAndSourcesAsync(movieId);

            if (movie == null)
                return movieGenres;

            movieGenres = movie.MovieGenres.ToList();

            var existingMovieGenreIds = movieGenres.Select(m => m.Genre.Id);

            // Alle zu entfernenden Genres
            var idsToRemove = (from c1 in existingMovieGenreIds select c1).Except(from c2 in genreIds select c2);
            foreach (string idToRemove in idsToRemove)
            {
                var movieGenreToRemove = movieGenres.Where(g => g.Genre.Id == idToRemove).FirstOrDefault();
                movieGenreToRemove.IsDeleted = true;
            }

            // Alle neuen Genres
            var idsToAdd = (from c1 in genreIds select c1).Except(from c2 in existingMovieGenreIds select c2);
            foreach (string idToAdd in idsToAdd)
            {
                var newMovieGenre = MovieGenre.CreateNew<MovieGenre>();
                newMovieGenre.Movie = movie;
                newMovieGenre.Genre = await Resolver.Resolve<IGenreService>().GetAsync(idToAdd);
                movieGenres.Add(newMovieGenre);
            }

            return movieGenres;
        }

        /// <summary>
        /// Ermittelt die Liste der Genres für den Movie und fügt diejenigen hinzu, deren Name noch nicht in der
        /// übergebenen Liste enthalten ist
        /// Es werden keine Genres entfernt
        /// Typische Anwednung: Bei Selektion eines Movie von einem Provider werden Genrenamen mitgeliefert, die
        /// dann dem Film hinzugefügt werden sollen
        /// </summary>
        /// <param name="movie">Film, dessen Genres erweitert werden sollem</param>
        /// <param name="genreNames">Namen der hinzuzufügenden Genres</param>
        /// <returns>Alle MovieGenres für den Film, also alte sowie neu hinzugefügte</returns>
        public async Task<ICollection<MovieGenre>> AddMovieGenres(Movie movie, IList<string> genreNames)
        {
            List<MovieGenre> movieGenres = new List<MovieGenre>();
            IGenreService genreService = Resolver.Resolve<IGenreService>();

            // Bestimmen der Genres, die dem Film neu zugeordnet werden müssen
            var existingGenres = movie.MovieGenres.Where(x => x.Movie == movie).Select(mg => mg.Genre);
            var existingGenreNames = existingGenres.Select(s => s.Name);
            var newGenreNames = (from genre in genreNames select genre).Except(from existingGenre in existingGenreNames select existingGenre);

            foreach (string genreName in newGenreNames)
            {
                // Genres anlegen, falls es noch nicht existiert
                Genre genre = await genreService.GetOrCreateGenre(genreName);

                MovieGenre movieGenre = MovieGenre.CreateNew<MovieGenre>();
                movieGenre.Movie = movie;
                movieGenre.Genre = genre;
                movieGenres.Add(movieGenre);
            }

            // Auch die bereits vorhandenen, zugeordneten Genres zurückgeben, damit diese angezeigt werden
            movieGenres.AddRange(movie.MovieGenres);

            return movieGenres;
        }

        public async Task<Movie> ApplyDtoData(Movie movie, MovieDbMovie movieDto)
        {
            movie.Title = movieDto.Title;
            movie.Description = movieDto.Overview;
            movie.ReleaseDate = movieDto.ReleaseDate;
            movie.LastSeen = MoviekusDefines.MinDate;
            movie.Rating = 0;
            movie.Runtime = movieDto.Runtime;
            movie.Cover = movieDto.Cover;
            movie.Homepage = movieDto.Homepage;
            movie.Trailer = movieDto.TrailerUrl;
            movie.MovieGenres = await AddMovieGenres(movie, movieDto.Genres);

            return movie;
        }

        protected override async Task InsertAsync(MoviekusDbContext context, Movie movie)
        {
            if (string.IsNullOrEmpty(movie.Title))
            {
                LogManager.GetCurrentClassLogger().Warn("Skipping insert of new movie because title is null");
                return;
            }                

            try
            {
                await context.Movies.AddAsync(movie);

                if (movie.Source != null)
                {
                    // Die Standard-Implementierung würde auch die Source als Added kennzeichnen und damit auch dort ein Insert ausführen
                    // Um dies zu verhindern, muss der Status der Source hier manuell umgesetzt werden
                    context.Entry(movie.Source).State = EntityState.Unchanged;
                }

                // Das gleiche gilt auch für die Genre-Zuordnungen
                var genres = movie.MovieGenres.Select(mg => mg.Genre);
                Parallel.ForEach(genres, g => { context.Entry(g).State = EntityState.Unchanged; });

                // IsNew für alle MovieGenes zurücksetzen, damit man direkt Änderungen vornehmen kann 
                // (sonst würde erneut Insert statt Update durchgeführt)
                Parallel.ForEach(movie.MovieGenres, mg => { mg.IsNew = false; });
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                throw;
            }
        }

        protected override void UpdateAsync(MoviekusDbContext context, Movie movie)
        {
            base.UpdateAsync(context, movie);

            try
            {
                if (movie.Source != null)
                {
                    // Stellt sicher, dass der ForeignKey aktualisiert wird, falls sich die Quelle geändert hat
                    context.Entry(movie.Source).State = EntityState.Modified;
                }

                // Spezialbehandlung für die n-m-Relation zwischen Movie und Genre
                foreach (MovieGenre movieGenre in movie.MovieGenres)
                {
                    if (movieGenre.IsNew && !movieGenre.IsDeleted)
                    {
                        if (context.Entry(movieGenre).State == EntityState.Detached)
                            context.Attach(movieGenre);
                        context.Entry(movieGenre).State = EntityState.Added;
                        movieGenre.IsNew = false;
                    }
                    else if (movieGenre.IsDeleted)
                    {
                        if (context.Entry(movieGenre).State == EntityState.Detached)
                            context.Attach(movieGenre);
                        context.Entry(movieGenre).State = EntityState.Deleted;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                throw;
            }
        }
    }
}
