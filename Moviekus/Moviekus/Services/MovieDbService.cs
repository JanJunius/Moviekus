﻿using Moviekus.Dto;
using Moviekus.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Moviekus.Services
{
    public class MovieDbService
    {
        private static MovieDbService TheInstance = null;

        private static Settings Settings;

        public static MovieDbService Ref
        {
            get
            {
                if (TheInstance == null)
                {
                    TheInstance = new MovieDbService();
                    Settings = new SettingsService().GetSettings();
                }
                return TheInstance;
            }
        }

        private MovieDbService()
        {
        }

        public async Task<IEnumerable<MovieDto>> SearchMovieAsync(string title)
        {
            MovieDbGenres genres = await GetGenres();
            List<MovieDto> movies = new List<MovieDto>();

            var client = new RestClient("https://api.themoviedb.org/3/search/movie");
            var request = new RestRequest(Method.GET);
            
            request.AddParameter("api_key", Settings.MovieDb_ApiKey);
            request.AddParameter("language", Settings.MovieDb_Language);
            request.AddParameter("query", title);
            request.AddParameter("page", "1");

            IRestResponse<MovieDbResponse> response = await client.ExecuteAsync<MovieDbResponse>(request);

            MovieDbResponse movieDbResponse = response.Data;

            if (movieDbResponse.results.Count < 1)
                return movies;

            foreach (MovieDbResult result in movieDbResponse.results)
                movies.Add(BuildMovieDto(result, genres));

            return movies.OrderByDescending(m => m.ReleaseDate);

            // Details nur nachladen, wenn genau ein Treffer vorliegt
            //if (movies.Count == 1)
            //    await FillMovieDetails(movies.First());

            //return movies;
        }

        public async Task FillMovieDetails(MovieDto movieDto)
        {
            string url = $"https://api.themoviedb.org/3/movie/{movieDto.MovieDbId}";
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);

            request.AddParameter("api_key", Settings.MovieDb_ApiKey);
            request.AddParameter("language", Settings.MovieDb_Language);

            IRestResponse<MovieDbDetails> details = await client.ExecuteAsync<MovieDbDetails>(request);

            if (details.Data != null)
                movieDto.Runtime = details.Data.runtime;
        }

        public byte[] GetMovieCover(MovieDto movieDto)
        {
            string url = $"https://image.tmdb.org/t/p/w500/{movieDto.CoverUri}";
            HttpWebRequest webRequest;
            WebResponse webResponse = null;

            try
            {
                webRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                webRequest.AllowWriteStreamBuffering = true;
                webRequest.Timeout = 30000;

                webResponse = webRequest.GetResponse();
                Stream stream = webResponse.GetResponseStream();
                return GetImageBytes(stream);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                webResponse?.Close();
            }
            return null;
        }

        private byte[] GetImageBytes(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        private MovieDto BuildMovieDto(MovieDbResult movieDbResult, MovieDbGenres movieDbGenres)
        {
            MovieDto movieDto = new MovieDto()
            {
                MovieDbId = movieDbResult.id,
                Overview = movieDbResult.overview,
                Title = movieDbResult.title,
                ReleaseDate = movieDbResult.release_date,
                CoverUri = movieDbResult.poster_path
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

            request.AddParameter("api_key", Settings.MovieDb_ApiKey);
            request.AddParameter("language", Settings.MovieDb_Language);

            IRestResponse<MovieDbGenres> response = await client.ExecuteAsync<MovieDbGenres>(request);

            return response.Data;
        }


    }
}