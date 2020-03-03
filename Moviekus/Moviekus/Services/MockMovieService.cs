using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moviekus.Models;

namespace Moviekus.Services
{
    public class MockMovieService : IMovieService<Movie>
    {
        readonly List<Movie> movies;

        public MockMovieService()
        {
            movies = new List<Movie>()
            {
                new Movie { Id = Guid.NewGuid().ToString(), Text = "First item", Description="This is an item description." },
                new Movie { Id = Guid.NewGuid().ToString(), Text = "Second item", Description="This is an item description." },
                new Movie { Id = Guid.NewGuid().ToString(), Text = "Third item", Description="This is an item description." },
                new Movie { Id = Guid.NewGuid().ToString(), Text = "Fourth item", Description="This is an item description." },
                new Movie { Id = Guid.NewGuid().ToString(), Text = "Fifth item", Description="This is an item description." },
                new Movie { Id = Guid.NewGuid().ToString(), Text = "Sixth item", Description="This is an item description." }
            };
        }

        public async Task<bool> AddMovieAsync(Movie movie)
        {
            movies.Add(movie);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateMovieAsync(Movie movie)
        {
            var oldMovie = movies.Where((Movie arg) => arg.Id == movie.Id).FirstOrDefault();
            movies.Remove(oldMovie);
            movies.Add(movie);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteMovieAsync(string id)
        {
            var oldMovie = movies.Where((Movie arg) => arg.Id == id).FirstOrDefault();
            movies.Remove(oldMovie);

            return await Task.FromResult(true);
        }

        public async Task<Movie> GetMovieAsync(string id)
        {
            return await Task.FromResult(movies.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Movie>> GetMoviesAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(movies);
        }
    }
}