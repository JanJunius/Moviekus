using Moviekus.Dto;
using Moviekus.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moviekus.Services
{
    public interface IMovieService : IService<Movie>
    {
        // Lädt alle Movies inkl. der zugehörigen Quellen
        Task<IEnumerable<Movie>> GetWithSourceAsync();

        // Lädt alle Movies inkl. der zugeordneten Genres
        Task<IEnumerable<Movie>> GetWithGenresAsync();

        // Lädt alle Movies inkl. der zugehörigen Quellen und Genres
        Task<IEnumerable<Movie>> GetWithGenresAndSourcesAsync();

        // Legt neuen Movie an oder speichert vorhandenen
        Task<Movie> SaveMovieAsync(Movie movie);

        Task<ICollection<MovieGenre>> GetMovieGenres(Movie movie, IList<string> genres);

    }
}
