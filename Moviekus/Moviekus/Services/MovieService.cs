using Microsoft.EntityFrameworkCore;
using Moviekus.EntityFramework;
using Moviekus.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviekus.Services
{
    public class MovieService : BaseService<Movie>, IMovieService
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

        public async Task<IEnumerable<Movie>> GetWithGenresAndSourcesAsync()
        {
            using (var context = new MoviekusDbContext())
            {

                return await context.Movies.Include(s => s.Source).Include(m => m.MovieGenres).ThenInclude(g => g.Genre).ToListAsync();
            }
        }

        public async Task<Movie> SaveMovieAsync(Movie movie)
        {
            await SaveChangesAsync(movie);
            return movie;
        }

        protected override async Task InsertAsync(MoviekusDbContext context, Movie movie)
        {
            await context.Movies.AddAsync(movie);

            // Die Standard-Implementierung würde auch die Source als Added kennzeichnen und damit auch dort ein Insert ausführen
            // Um dies zu verhindern, muss der Status der Source hier manuell umgesetzt werden
            context.Entry(movie.Source).State = EntityState.Unchanged;
        }

        protected override void UpdateAsync(MoviekusDbContext context, Movie movie)
        {
            base.UpdateAsync(context, movie);

            // Stellt sicher, dass der ForeignKey aktualisiert wird, falls sich die Quelle geändert hat
            context.Entry(movie.Source).State = EntityState.Modified;

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
    }
}
