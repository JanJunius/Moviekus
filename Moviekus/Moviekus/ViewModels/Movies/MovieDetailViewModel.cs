using Acr.UserDialogs;
using Moviekus.Dto;
using Moviekus.Models;
using Moviekus.ServiceContracts;
using Moviekus.Views.Movies;
using NLog;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Moviekus.ViewModels.Movies
{
    public class MovieDetailViewModel : BaseViewModel
    {
        protected IMovieService MovieService;
        
        public MovieDetailViewModel(IMovieService movieService)
        {
            MovieService = movieService;
            MovieDetails = new MovieDetailDto();
        }

        public MovieDetailDto MovieDetails { get; set; }

        public ICommand EditCommand => new Command(async () =>
        {
            var movieEditView = Resolver.Resolve<MovieEditPage>();
            var viewModel = movieEditView.BindingContext as MovieEditViewModel;
            viewModel.Movie = MovieDetails.Movie;
            viewModel.Title = "Film bearbeiten";

            viewModel.OnMovieChanged += (object sender, Movie movie) =>
            {
                MovieDetails.Movie = movie;
                RaisePropertyChanged(nameof(MovieDetailDto));
                // Die folgenden Properties stammen nicht aus Movie, sondern dem ViewModel selbst und benötigen daher ein eigenes Event
                RaisePropertyChanged(nameof(MovieDetails.ReleaseDateText));
                RaisePropertyChanged(nameof(MovieDetails.LastSeenText));
                RaisePropertyChanged(nameof(Genres));
                RaisePropertyChanged(nameof(MovieDetails.HasHomepage));
                RaisePropertyChanged(nameof(MovieDetails.HasTrailer));
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
                await MovieService.DeleteAsync(MovieDetails.Movie);
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
    }
}
