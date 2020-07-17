using Moviekus.Dto;
using Moviekus.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moviekus.ServiceContracts
{
    public interface IMovieService : IBaseService<Movie>
    {
        Task<IList<Movie>> GetWithGenresAndSourcesAsync(MovieSortOrder sortOrder);
        Task<Movie> GetWithGenresAndSourcesAsync(string id);

        MovieDetailDto GetMovieDetails(Movie Movie);

        Task<Movie> SaveMovieAsync(Movie movie);

        Task<ICollection<MovieGenre>> AddMovieGenres(Movie movie, IList<string> genreNames);
        Task<ICollection<MovieGenre>> SyncMovieGenres(string movieId, IList<string> genreIds);

    }
}
