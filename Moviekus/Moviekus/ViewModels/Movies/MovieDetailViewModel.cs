using Acr.UserDialogs;
using Moviekus.Models;
using Moviekus.Services;
using Moviekus.Views.Movies;
using System.Linq;
using System.Windows.Input;
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
                return genres.Length > 1 ? genres.Substring(0, genres.Length-2) : genres;
            }
            set { }
        }
        private Movie _movie;

        public Movie Movie
        {
            get { return _movie; }
            set
            {
                _movie = value;
            }
        }

        public ICommand EditCommand => new Command(async () =>
        {
            var movieEditView = Resolver.Resolve<MovieEditPage>();
            var viewModel = movieEditView.BindingContext as MovieEditViewModel;
            viewModel.Movie = Movie;

            viewModel.OnMovieChanged += (object sender, Movie movie) => {
                Movie = movie;
                RaisePropertyChanged(nameof(Movie));
                RaisePropertyChanged(nameof(Genres));
            };

            await Navigation.PushAsync(movieEditView);
        });

        public ICommand DeleteCommand => new Command(async () =>
        {
            var result = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig
            {
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

        public MovieDetailViewModel(MovieService movieService)
        {
            MovieService = movieService;
        }
    }
}
