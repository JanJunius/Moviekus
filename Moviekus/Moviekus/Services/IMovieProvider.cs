using Moviekus.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Moviekus.Services
{
    public interface IMovieProvider
    {
        string Name { get; }

        Task<IEnumerable<MovieDto>> SearchMovieAsync(string title);
        Task FillMovieDetails(MovieDto movieDto);
        Task FillMovieTrailer(MovieDto movieDto);
        byte[] GetMovieCover(MovieDto movieDto);
    }
}
