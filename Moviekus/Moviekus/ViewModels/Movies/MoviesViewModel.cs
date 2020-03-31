using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using Moviekus.Models;
using Moviekus.Views.Movies;
using Moviekus.Services;
using System.Linq;
using System.Windows.Input;

namespace Moviekus.ViewModels.Movies
{
    public class MoviesViewModel : BaseViewModel
    {
        private IMovieService MoviesService;

        public ObservableCollection<MoviesItemViewModel> Movies { get; set; }

        public ICommand LoadMoviesCommand => new Command(async () =>
        {
            await LoadMovies();
        });

        public ICommand AddMovieCommand => new Command(async () =>
        {
            var movieEditView = Resolver.Resolve<MovieEditPage>();
            var viewModel = movieEditView.BindingContext as MovieEditViewModel;
            viewModel.Title = "Neuer Film";
            viewModel.Movie = CreateNewMovie();

            await Navigation.PushAsync(movieEditView);
        });

        private Movie CreateNewMovie()
        {
            Movie movie = Movie.CreateNew<Movie>();
            movie.Source = SourceService.GetDefaultSource();
            movie.LastSeen = DateTime.Today;
            movie.ReleaseDate = DateTime.Today;
            return movie;
        }

        public MoviesViewModel(MovieService moviesService)
        {
            Title = "Filme";
            Movies = new ObservableCollection<MoviesItemViewModel>();
            MoviesService = moviesService;
            
            // Wiederspiegeln der Datenbankänderungen in der Liste
            moviesService.OnModelInserted += (sender, movie) => Movies.Add(CreateMoviesItemViewModel(movie));
            moviesService.OnModelUpdated += async (sender, movie) => await LoadMovies();
            moviesService.OnModelDeleted += (sender, movie) => Movies.Remove(CreateMoviesItemViewModel(movie));
        }

        public MoviesItemViewModel SelectedItem
        {
            get { return null; }
            set
            {
                if (value == null)
                    return;

                Device.BeginInvokeOnMainThread(async () => await OpenDetailPage(value));
                RaisePropertyChanged(nameof(SelectedItem));
            }
        }
        
        private async Task OpenDetailPage(MoviesItemViewModel miViewModel)
        {
            var detailView = Resolver.Resolve<MovieDetailPage>();
            var viewModel = detailView.BindingContext as MovieDetailViewModel;
            viewModel.Movie = miViewModel.Movie;
            viewModel.Title = "Filmdetails";

            await Navigation.PushAsync(detailView);
        }

        private async Task LoadMovies()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Movies.Clear();
                //var movies = await MoviesService.GetAsync();
                //var movies = await MoviesService.GetWithSourceAsync();
                var movies = await MoviesService.GetWithGenresAndSourcesAsync();

                var itemViewModels = movies.Select(m => CreateMoviesItemViewModel(m));
                Movies = new ObservableCollection<MoviesItemViewModel>(itemViewModels);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private MoviesItemViewModel CreateMoviesItemViewModel(Movie movie)
        {
            var moviesItemViewModel = new MoviesItemViewModel(movie);
            //moviesItemViewModel.MovieStatusChanged += MovieStatusChanged;
            return moviesItemViewModel;
        }

    }
}