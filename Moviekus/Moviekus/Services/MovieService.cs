using Microsoft.EntityFrameworkCore;
using Moviekus.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Moviekus.Services
{
    public class MovieService : BaseService<Movie>, IMovieService
    {
        public async Task<IEnumerable<Movie>> GetWithSourceAsync()
        {
            return await db.Movies.Include(m => m.Source).ToListAsync();
        }
    }
}
