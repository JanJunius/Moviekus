using Moviekus.Dto;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moviekus.Services
{
    public class MovieDbService
    {
        private const string api_key = "121d3dae50d820edc0329e0bcf02c712";
        private const string language = "de-DE";

        public async Task<IEnumerable<MovieDto>> SearchMovie(string title)
        {
            MovieDbGenres genres = await GetGenres();
            List<MovieDto> movies = new List<MovieDto>();

            var client = new RestClient("https://api.themoviedb.org/3/search/movie");
            var request = new RestRequest(Method.GET);
            
            request.AddParameter("api_key", api_key);
            request.AddParameter("language", language);
            request.AddParameter("query", title);
            request.AddParameter("page", "1");

            IRestResponse<MovieDbResponse> response = await client.ExecuteAsync<MovieDbResponse>(request);

            MovieDbResponse movieDbResponse = response.Data;

            if (movieDbResponse.results.Count < 1)
                return movies;

            foreach (MovieDbResult result in movieDbResponse.results)
                movies.Add(BuildMovieDto(result, genres));

            // Details nur nachladen, wenn genau ein Treffer vorliegt
            if (movies.Count == 1)
                await FillMovieDetails(movies.First());

            return movies;
        }

        public async Task FillMovieDetails(MovieDto movieDto)
        {
            string url = $"https://api.themoviedb.org/3/movie/{movieDto.MovieDbId}";
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);

            request.AddParameter("api_key", api_key);
            request.AddParameter("language", language);

            IRestResponse<MovieDbDetails> details = await client.ExecuteAsync<MovieDbDetails>(request);

            if (details.Data != null)
                movieDto.Runtime = details.Data.runtime;
        }

        private MovieDto BuildMovieDto(MovieDbResult movieDbResult, MovieDbGenres movieDbGenres)
        {
            MovieDto movieDto = new MovieDto()
            {
                MovieDbId = movieDbResult.id,
                Overview = movieDbResult.overview,
                Title = movieDbResult.title,
                ReleaseDate = movieDbResult.release_date
                
            };

            foreach (string genreId in movieDbResult.genre_ids)
            {
                var genre = movieDbGenres.genres.Where(g => g.id == genreId).FirstOrDefault();
                if (genre != null)
                    movieDto.Genres.Add(genre.name);
            }

            return movieDto;
        }

        private async Task<MovieDbGenres> GetGenres()
        {
            Dictionary<string, string> genres = new Dictionary<string, string>();

            MovieDbResponse movieDbItem = new MovieDbResponse();

            var client = new RestClient("https://api.themoviedb.org/3/genre/movie/list");
            var request = new RestRequest(Method.GET);

            request.AddParameter("api_key", api_key);
            request.AddParameter("language", language);

            IRestResponse<MovieDbGenres> response = await client.ExecuteAsync<MovieDbGenres>(request);

            return response.Data;
        }
    }
}
