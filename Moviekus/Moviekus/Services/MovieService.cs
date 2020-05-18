using Microsoft.EntityFrameworkCore;
using Moviekus.EntityFramework;
using Moviekus.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviekus.Services
{
    public class MovieService : BaseService<Movie>
    {
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

        public async Task<IEnumerable<Movie>> GetWithGenresAndSourcesAsync(MovieSortOrder sortOrder)
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
                        return movies.OrderBy(m => m.Title);
                    case MovieSortOrder.LastSeen:
                        return movies.OrderBy(m => m.LastSeen);
                    case MovieSortOrder.Rating:
                        return movies.OrderByDescending(m => m.Rating);
                    case MovieSortOrder.ReleaseDate:
                        return movies.OrderByDescending(m => m.ReleaseDate);
                    case MovieSortOrder.Runtime:
                        return movies.OrderBy(m => m.Runtime);
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

        public async Task<Movie> SaveMovieAsync(Movie movie)
        {
            await SaveChangesAsync(movie);
            return movie;
        }

        public async Task<ICollection<MovieGenre>> GetMovieGenres(Movie movie, IList<string> genres)
        {
            List<MovieGenre> movieGenres = new List<MovieGenre>();
            GenreService genreService = new GenreService();

            // Bestimmen der Genres, die dem Film neu zugeordnet werden müssen
            var existingGenres = movie.MovieGenres.Where(x => x.Movie == movie).Select(mg => mg.Genre);
            var existingGenreNames = existingGenres.Select(s => s.Name);
            var newGenreNames = (from genre in genres select genre).Except(from existingGenre in existingGenreNames select existingGenre);

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
