using Moviekus.Dto;
using Moviekus.Models;
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
    public class ImDbService : IMovieProvider
    {
        private static ImDbService TheInstance = null;

        private static Settings Settings;

        public static ImDbService Ref
        {
            get
            {
                if (TheInstance == null)
                {
                    TheInstance = new ImDbService();
                    Settings = new SettingsService().GetSettings();
                }
                return TheInstance;
            }
        }

        private ImDbService()
        {
        }

        private string ApiKey => Settings.ImDb_ApiKey;

        private string Lang => Settings.ImDb_Language;

        public string Name => "ImDb";

        public async Task<IEnumerable<MovieDto>> SearchMovieAsync(string title)
        {
            List<MovieDto> movies = new List<MovieDto>();

            LogManager.GetCurrentClassLogger().Info($"Searching ImDb for '{title}'...");

            try
            {
                var client = new RestClient($"https://imdb-api.com/{Lang}/API/SearchMovie/{ApiKey}/{title}");
                var request = new RestRequest(Method.GET);

                IRestResponse<ImDbResponse> response = await client.ExecuteAsync<ImDbResponse>(request);

                ImDbResponse imDbResponse = response.Data;

                // ImDb lässt nur 100 API-Calls pro Tag zu und liefert dann null als result
                if (imDbResponse.results == null)
                    throw new MaximumUsageException("Maximum usage of ImDb-Service reached!");

                if (imDbResponse.results.Count < 1)
                    return movies;

                // Die Abfragen in ImDb sind mengenmäßig begrenzt und da zu jedem Ergebnis die Details noch abgefragt werden müssen,
                // werden die Treffer auf maximal 5 begrenzt
                foreach (ImDbResult result in imDbResponse.results.GetRange(0,5))
                {
                    var dto = BuildMovieDto(result);
                    await LoadMovieDetails(dto);
                    movies.Add(dto);
                }                    
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                throw;
            }

            LogManager.GetCurrentClassLogger().Info($"Finished searching ImDb for '{title}'.");

            return movies.OrderByDescending(m => m.ReleaseDate);
        }

        public Task FillMovieDetails(MovieDto movieDto)
        {
            // Das Nachladen der Details macht bei ImDb keinen Sinn, da die Grunddaten so dünn sind, dass man schon für die Auswahlliste alle Details ziehen muss
            return Task.FromResult(0);
        }

        public Task FillMovieTrailer(MovieDto movieDto)
        {
            // Nicht notwendig, da Trailer-Infos schon mit den normalen Details geliefert werden
            return Task.FromResult(0);
        }

        public byte[] GetMovieCover(MovieDto movieDto)
        {
            if (string.IsNullOrEmpty(movieDto.CoverUri))
            {
                LogManager.GetCurrentClassLogger().Info($"No cover available on ImDb for '{movieDto.Title}'...");
                return null;
            }

            HttpWebRequest webRequest;
            WebResponse webResponse = null;

            LogManager.GetCurrentClassLogger().Info($"Getting cover from ImDb for '{movieDto.Title}'...");

            try
            {
                webRequest = (HttpWebRequest)HttpWebRequest.Create(movieDto.CoverUri);
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
                LogManager.GetCurrentClassLogger().Info($"Finished getting cover from ImDb for '{movieDto.Title}'...");
            }
            return null;
        }

        private async Task LoadMovieDetails(MovieDto movieDto)
        {
            string url = $"https://imdb-api.com/{Lang}/API/Title/{ApiKey}/{movieDto.ProviderMovieId}/Posters,Trailer";
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);

            LogManager.GetCurrentClassLogger().Info($"Searching ImDb for details on '{movieDto.Title}'...");

            try
            {
                IRestResponse<ImDbDetails> details = await client.ExecuteAsync<ImDbDetails>(request);

                if (details.Data != null)
                {
                    movieDto.Runtime = details.Data.runtimeMins;
                    movieDto.Overview = details.Data.plot;
                    
                    if (details.Data.releaseDate.Year > 1900)
                        movieDto.ReleaseDate = details.Data.releaseDate;

                    foreach (string genre in details.Data.genres.Split(','))
                    {
                        movieDto.Genres.Add(genre.Trim());
                    }

                    if (details.Data.trailer != null)
                        movieDto.TrailerUrl = details.Data.trailer.link;
                }
                LogManager.GetCurrentClassLogger().Info($"Finished searching ImDb for details on '{movieDto.Title}'...");
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                throw;
            }
        }

        private MovieDto BuildMovieDto(ImDbResult imDbResult)
        {
            MovieDto movieDto = new MovieDto()
            {
                ProviderMovieId = imDbResult.id,
                Title = imDbResult.title,
                ReleaseDate = MoviekusDefines.MinDate,
                Overview = "<keine Beschreibung verfügbar>",
                CoverUri = imDbResult.image
            };

            return movieDto;
        }

    }
}
