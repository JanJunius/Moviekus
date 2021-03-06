﻿using Moviekus.Dto;
using Moviekus.Dto.MovieDb;
using Moviekus.Models;
using Moviekus.ServiceContracts;
using NLog;
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
    public class MovieDbService : IMovieProvider
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
                    Settings = Resolver.Resolve<ISettingsService>().GetSettings();
                }
                return TheInstance;
            }
        }

        public string Name => "MovieDb";

        private MovieDbService()
        {
        }

        public async Task<IEnumerable<MovieDbMovie>> SearchMovieAsync(string title)
        {
            MovieDbGenres genres = await GetGenres();
            List<MovieDbMovie> movies = new List<MovieDbMovie>();

            if (string.IsNullOrEmpty(title))
                return movies;

            LogManager.GetCurrentClassLogger().Info($"Searching MovieDb for '{title}'...");

            try
            {
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
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
            }

            LogManager.GetCurrentClassLogger().Info($"Finished searching MovieDb for '{title}'.");

            return movies.OrderByDescending(m => m.ReleaseDate);
        }

        public async Task FillMovieDetails(MovieDbMovie movieDto)
        {
            string url = $"https://api.themoviedb.org/3/movie/{movieDto.ProviderMovieId}";
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);

            request.AddParameter("api_key", Settings.MovieDb_ApiKey);
            request.AddParameter("language", Settings.MovieDb_Language);

            LogManager.GetCurrentClassLogger().Info($"Searching MovieDb for details on '{movieDto.Title}'...");

            try
            {
                IRestResponse<MovieDbDetails> details = await client.ExecuteAsync<MovieDbDetails>(request);

                if (details.Data != null)
                {
                    movieDto.Runtime = details.Data.runtime;
                    movieDto.Homepage = details.Data.homepage;
                }
                LogManager.GetCurrentClassLogger().Info($"Finished searching MovieDb for details on '{movieDto.Title}'...");
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                throw;
            }
        }

        public async Task<MovieDbMovie> GetMovieAsync(string providerMovieId)
        {
            string url = $"https://api.themoviedb.org/3/movie/{providerMovieId}";
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);

            request.AddParameter("api_key", Settings.MovieDb_ApiKey);
            request.AddParameter("language", Settings.MovieDb_Language);

            LogManager.GetCurrentClassLogger().Info($"Searching MovieDb for details on movie with id: '{providerMovieId}'...");

            try
            {
                IRestResponse<MovieDbDetails> details = await client.ExecuteAsync<MovieDbDetails>(request);
                LogManager.GetCurrentClassLogger().Info($"Finished searching MovieDb for details on movie with id: '{providerMovieId}'...");

                if (details.Data != null)
                {
                    MovieDbMovie movieDto;
                    movieDto = BuildMovieDto(details.Data);
                    movieDto.Cover = GetMovieCover(movieDto);
                    await FillMovieTrailer(movieDto);
                    return movieDto;
                }                    
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                throw;
            }
            return null;
        }

        public async Task FillMovieTrailer(MovieDbMovie movieDto)
        {
            string url = $"https://api.themoviedb.org/3/movie/{movieDto.ProviderMovieId}/videos";
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);

            request.AddParameter("api_key", Settings.MovieDb_ApiKey);

            LogManager.GetCurrentClassLogger().Info($"Searching MovieDb for trailer on '{movieDto.Title}'...");

            try
            {
                IRestResponse<MovieDbVideos> details = await client.ExecuteAsync<MovieDbVideos>(request);
                string trailerKey = string.Empty;

                if (details.Data != null)
                {
                    // Gibt es deutsche YouTube-Videos, nehmen wir das größte deutsche, sonst das größte englische, sonst gar nichts
                    if (details.Data.results.Any(d => d.iso_639_1 == "de"))
                        trailerKey = details.Data.results.Where(d => d.site == "YouTube" && d.iso_639_1 == "de").OrderByDescending(r => r.size).First().key;
                    else if (details.Data.results.Any(d => d.iso_639_1 == "en"))
                        trailerKey = details.Data.results.Where(d => d.site == "YouTube" && d.iso_639_1 == "en").OrderByDescending(r => r.size).First().key;

                    if (!string.IsNullOrEmpty(trailerKey))
                        movieDto.TrailerUrl = $"https://www.youtube.com/watch?v={trailerKey}";
                }
                
                LogManager.GetCurrentClassLogger().Info($"Finished searching MovieDb for trailer on '{movieDto.Title}'.");
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                throw;
            }
        }

        public byte[] GetMovieCover(MovieDbMovie movieDto)
        {
            if (string.IsNullOrEmpty(movieDto.CoverUri))
                return null;

            string url = $"https://image.tmdb.org/t/p/w500/{movieDto.CoverUri}";
            HttpWebRequest webRequest;
            WebResponse webResponse = null;

            LogManager.GetCurrentClassLogger().Info($"Getting cover from MovieDb for '{movieDto.Title}'...");

            try
            {
                webRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                webRequest.AllowWriteStreamBuffering = true;
                webRequest.Timeout = 30000;

                webResponse = webRequest.GetResponse();
                Stream stream = webResponse.GetResponseStream();
                return Utils.GetImageBytes(stream);
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
            }
            finally
            {
                webResponse?.Close();
                LogManager.GetCurrentClassLogger().Info($"Finished getting cover from MovieDb for '{movieDto.Title}'...");
            }
            return null;
        }

        private MovieDbMovie BuildMovieDto(MovieDbResult movieDbResult, MovieDbGenres movieDbGenres)
        {
            MovieDbMovie movieDto = new MovieDbMovie()
            {
                ProviderMovieId = movieDbResult.id,
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

        private MovieDbMovie BuildMovieDto(MovieDbDetails movieDbDetails)
        {
            MovieDbMovie movieDto = new MovieDbMovie()
            {
                ProviderMovieId = movieDbDetails.id,
                Overview = movieDbDetails.overview,
                Title = movieDbDetails.title,
                ReleaseDate = movieDbDetails.release_date,
                CoverUri = movieDbDetails.poster_path,
                Runtime = movieDbDetails.runtime,
                Homepage = movieDbDetails.homepage
            };

            foreach (var genre in movieDbDetails.Genres)
            {
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

            LogManager.GetCurrentClassLogger().Info("Getting genres from MovieDb...");

            try
            {
                IRestResponse<MovieDbGenres> response = await client.ExecuteAsync<MovieDbGenres>(request);
                LogManager.GetCurrentClassLogger().Info("Finished getting genres from MovieDb");

                return response.Data;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                throw ex;
            }
        }
    }
}
