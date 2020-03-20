using Moviekus.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moviekus.Services
{
    public interface IMovieService : IService<Movie>
    {
        Task<IEnumerable<Movie>> GetWithSourceAsync();

    }
}
