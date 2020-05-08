using Acr.UserDialogs;
using Moviekus.Models;
using Moviekus.Services;
using Moviekus.Views.Movies;
using NLog;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Moviekus.ViewModels.Movies
{
    public class MovieDetailViewModel : BaseViewModel
    {
        MovieService MovieService;

        public string Genres
        {
            get
            {
                string genres = string.Empty;
                if (Movie != null)
                {
                    var genreList = Movie.MovieGenres.Select(g => g.Genre);
                    genreList.ForEach(g => genres += g.Name + "; ");
                }
                return genres.Length > 1 ? genres.Substring(0, genres.Length - 2) : genres;
            }
            set { }
        }

        public Movie Movie { get; set; }

        public string ReleaseDateText => Movie != null && Movie.ReleaseDate != MoviekusDefines.MinDate ? Movie.ReleaseDate.ToString("d", MoviekusDefines.MoviekusCultureInfo) : "<unbekannt>";

        public string LastSeenText => Movie != null && Movie.LastSeen != MoviekusDefines.MinDate? Movie.LastSeen.ToString("d", MoviekusDefines.MoviekusCultureInfo) : "<noch nicht gesehen>";

        public ICommand EditCommand => new Command(async () =>
        {
            var movieEditView = Resolver.Resolve<MovieEditPage>();
            var viewModel = movieEditView.BindingContext as MovieEditViewModel;
            viewModel.Movie = Movie;
            viewModel.Title = "Film bearbeiten";

            viewModel.OnMovieChanged += (object sender, Movie movie) =>
            {
                Movie = movie;
                RaisePropertyChanged(nameof(Movie));
                RaisePropertyChanged(nameof(ReleaseDateText));
                RaisePropertyChanged(nameof(LastSeenText));
                RaisePropertyChanged(nameof(Genres));
            };

            await Navigation.PushAsync(movieEditView);
        });

        public ICommand DeleteCommand => new Command(async () =>
        {
            var result = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig
            {
                Title = "Film löschen",
                Message = "Diesen Film wirklich löschen?",
                OkText = "Ja",
                CancelText = "Nein"
            });
            if (result)
            {
                await MovieService.DeleteAsync(Movie);
                RaisePropertyChanged(nameof(Movie));
                await Navigation.PopAsync();
            }
        });

        public ICommand HomepageClickedCommand => new Command<string>(async (url) =>
        {
            try
            {
                if (!string.IsNullOrEmpty(url))
                    await Browser.OpenAsync(new Uri(url), BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
            }
        });

        public ICommand TrailerClickedCommand => new Command<string>(async (trailerUrl) =>
        {
            try
            {
                if (!string.IsNullOrEmpty(trailerUrl))
                {
                    Uri uri = new Uri(trailerUrl);
                    await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
                }                
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
            }
        });

        public MovieDetailViewModel(MovieService movieService)
        {
            MovieService = movieService;
        }

        public bool HasHomepage => Movie != null ? !string.IsNullOrEmpty(Movie.Homepage) : false;
        public bool HasTrailer => Movie != null ? !string.IsNullOrEmpty(Movie.Trailer) : false;

    }
}
