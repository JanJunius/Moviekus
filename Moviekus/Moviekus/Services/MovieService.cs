using Microsoft.EntityFrameworkCore;
using Moviekus.EntityFramework;
using Moviekus.Models;
using System.Collections.Generic;
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

    }
}
