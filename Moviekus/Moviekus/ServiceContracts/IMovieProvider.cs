using Moviekus.Dto;
using Moviekus.Dto.MovieDb;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moviekus.ServiceContracts
{
    public interface IMovieProvider
    {
        string Name { get; }

        Task<IEnumerable<MovieDbMovie>> SearchMovieAsync(string title);
        Task<MovieDbMovie> GetMovieAsync(string providerMovieId);
        Task FillMovieDetails(MovieDbMovie movieDto);
        Task FillMovieTrailer(MovieDbMovie movieDto);
        byte[] GetMovieCover(MovieDbMovie movieDto);
    }
}
