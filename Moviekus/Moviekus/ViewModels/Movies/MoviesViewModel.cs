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
            var movieDetailView = Resolver.Resolve<MovieDetailPage>();
            var viewModel = movieDetailView.BindingContext as MovieDetailViewModel;
            viewModel.Movie = new Movie();
            viewModel.Title = "Neuen Film erfassen";

            await Navigation.PushAsync(movieDetailView);
        });

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

                // Manually deselect item.
                //MoviesListView.SelectedItem = null;

            }
        }
        
        private async Task OpenDetailPage(MoviesItemViewModel miViewModel)
        {
            var detailView = Resolver.Resolve<MovieDetailPage>();
            var viewModel = detailView.BindingContext as MovieDetailViewModel;
            viewModel.Movie = miViewModel.Movie;

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
                var movies = await MoviesService.GetWithSourceAsync();

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

        /*
        private async Task LoadData()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Movies.Clear();
                var movies = await DataStore.GetMoviesAsync(true);

                // Aus der DB wird eine Liste von Movies geliefert, die einzelnen Elemente der Filmliste haben aber
                // ihr eigenes ViewModel, um z.B. eigene Commands oder Texte daran binden zu können
                // Daher wird aus der Liste der Movies eine Liste mit MoviesItemViewModels gemacht
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
        */

        private MoviesItemViewModel CreateMoviesItemViewModel(Movie movie)
        {
            var moviesItemViewModel = new MoviesItemViewModel(movie);
            //moviesItemViewModel.MovieStatusChanged += MovieStatusChanged;
            return moviesItemViewModel;
        }

        private void MovieStatusChanged(object sender, EventArgs e)
        {
            if (sender is MoviesItemViewModel miViewModel)
            {
                /* Abspeichern der Änderung auf 2 Varianten möglich:
                 * 1. a) Diese Methode als sync kennzeichnen
                 0*0410+
                 41444444474444b) await repository.UpdateItem(tdiViewModel.Item);
                 * 2. Asynchrones Ausführen der Aktion in einem neuen Task, siehe unten   
                 *    Dann muss diese Methode nicht mit async gekennzeichnet werden
                */
                //Task.Run(async () => await repository.UpdateItem(tdiViewModel.Item));
            }
        }

    }
}