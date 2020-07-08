using Moviekus.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moviekus.ServiceContracts
{
    public interface IMovieService : IBaseService<Movie>
    {
        Task<IList<Movie>> GetWithGenresAndSourcesAsync(MovieSortOrder sortOrder);
        Task<Movie> GetWithGenresAndSourcesAsync(string id);


        Task<Movie> SaveMovieAsync(Movie movie);

        Task<ICollection<MovieGenre>> GetMovieGenres(Movie movie, IList<string> genres);
    }
}
