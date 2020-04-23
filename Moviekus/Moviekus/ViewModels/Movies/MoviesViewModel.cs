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
using Moviekus.Views.Filter;
using Moviekus.ViewModels.Filter;
using System.Linq.Expressions;
using System.Collections.Generic;

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

        public ICommand FilterCommand => new Command(async () =>
        {
            var filterView = Resolver.Resolve<FilterSelectionPage>();
            var viewModel = filterView.BindingContext as FilterSelectionViewModel;
            viewModel.Title = "Filter wählen";
            viewModel.LoadFilterCommand.Execute(null);

            viewModel.FilterSelected += async (sender, filter) => 
            {
                if (filter != null)
                {
                    Title = $"Filme ({filter.Name})";
                    await ApplyFilter(filter);
                }
                else
                {
                    Title = "Filme";
                    await RemoveFilter();
                }                
            };

            await Navigation.PushAsync(filterView);
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
            try
            {
                //var movies = await MoviesService.GetAsync();
                //var movies = await MoviesService.GetWithSourceAsync();
                var movies = await MoviesService.GetWithGenresAndSourcesAsync();

                Movies = new ObservableCollection<MoviesItemViewModel>(movies.Select(m => CreateMoviesItemViewModel(m)));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private MoviesItemViewModel CreateMoviesItemViewModel(Movie movie)
        {
            var moviesItemViewModel = new MoviesItemViewModel(movie);
            //moviesItemViewModel.MovieStatusChanged += MovieStatusChanged;
            return moviesItemViewModel;
        }

        private async Task ApplyFilter(Models.Filter filter)
        {
            await RemoveFilter();

            var titleEntries = filter.FilterEntries.Where(v => v.FilterEntryType.Property == FilterEntryProperty.Title);

            try
            {
                Movies = new ObservableCollection<MoviesItemViewModel>(Movies.AsQueryable().Where(FilterBuilder.Ref.BuildFilter(filter)));
            }
            catch(Exception ex)
            {
                string test = ex.ToString();
            }
            
            /*
            foreach (var filterEntry in filter.FilterEntries)
            {
                if (filterEntry.FilterEntryType.Property == FilterEntryProperty.Remarks)
                {
                    //Movies = new ObservableCollcetion<Movie>(Movies.Select(m => m.Title.Contains(filterEntry.ValueFrom)));

                    Movies = new ObservableCollection<MoviesItemViewModel>(Movies.Where(m => m.Movie.Remarks != null && m.Movie.Remarks.Contains(filterEntry.ValueFrom)));
                }
            }
            */
        }

        private async Task RemoveFilter()
        {
            await LoadMovies();
        }




    }
}