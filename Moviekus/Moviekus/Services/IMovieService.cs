using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moviekus.Services
{
    public interface IMovieService<T>
    {
        Task<bool> AddMovieAsync(T movie);
        Task<bool> UpdateMovieAsync(T movie);
        Task<bool> DeleteMovieAsync(string id);
        Task<T> GetMovieAsync(string id);
        Task<IEnumerable<T>> GetMoviesAsync(bool forceRefresh = false);

    }
}
