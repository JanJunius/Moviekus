using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Xamarin.Forms;

using Moviekus.Models;
using Moviekus.Views.Movies;
using Moviekus.FilterBuilder;
using System.Linq;
using System.Windows.Input;
using Moviekus.Views.Filter;
using Moviekus.ViewModels.Filter;
using System.Linq.Expressions;
using System.Collections.Generic;
using NLog;
using Acr.UserDialogs;
using Moviekus.OneDrive;
using Moviekus.ServiceContracts;

namespace Moviekus.ViewModels.Movies
{
    public class MoviesViewModel : BaseViewModel
    {
        private IMovieService MoviesService;

        private MovieSortOrder MovieSortOrder = MovieSortOrder.Title;
        private Models.Filter MovieFilter = null;

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
                MovieFilter = null;
                if (filter != null)
                    MovieFilter = await Resolver.Resolve<IFilterService>().SetDefault(filter);
                else await Resolver.Resolve<IFilterService>().ResetDefault();
                await LoadMovies();
            };

            await Navigation.PushAsync(filterView);
        });

        public ICommand OneDriveCommand => new Command(async () =>
        {
            var actionResult = await UserDialogs.Instance.ActionSheetAsync("Möchten Sie die Datenbank hoch- oder herunterladen?", "Abbrechen", null, null, "Hochladen", "Herunterladen");
            if (actionResult == "Hochladen")
            {
                Device.BeginInvokeOnMainThread(() => UserDialogs.Instance.ShowLoading("Lade Datenbank hoch..."));
                bool success = await DbFileManager.UploadDbToOneDrive();
                Device.BeginInvokeOnMainThread(() => UserDialogs.Instance.HideLoading());
                if (success)
                    UserDialogs.Instance.Toast("Upload erfolgreich");
                else await UserDialogs.Instance.AlertAsync("Datenbank konnte nicht auf OneDrive geladen werden (Details siehe Log).", "OneDrive-Upload");
            }
            else if (actionResult == "Herunterladen")
            {
                var result = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig
                {
                    Title = "Datenbank-Download",
                    Message = "ACHTUNG! Hierdurch werden alle lokale Daten überschrieben! Fortfahren?",
                    OkText = "Ja",
                    CancelText = "Nein"
                });
                if (result)
                {
                    Device.BeginInvokeOnMainThread(() => UserDialogs.Instance.ShowLoading("Lade Datenbank herunter..."));
                    bool success = await DbFileManager.DownloadDbFromOneDrive();
                    Device.BeginInvokeOnMainThread(() => UserDialogs.Instance.HideLoading());
                    if (success)
                    {
                        UserDialogs.Instance.Toast("Download erfolgreich");
                        LoadMoviesCommand.Execute(null);
                    }
                    else await UserDialogs.Instance.AlertAsync("Datenbank konnte nicht auf OneDrive geladen werden (Details siehe Log).", "OneDrive-Upload");
                }
            }
        });

        public ICommand OrderCommand => new Command(async () =>
        {
            var actionResult = await UserDialogs.Instance.ActionSheetAsync("Nach was soll sortiert werden?", "Abbrechen", null, null, MovieSortOrderHelper.GetDisplayNames());

            MovieSortOrder = MovieSortOrderHelper.GetSortOrderFromDisplayName(actionResult);

            await LoadMovies();
        });

        public ICommand SearchCommand => new Command(async () =>
        {
            var promptResult = await UserDialogs.Instance.PromptAsync("Titel", "Schnellsuche nach Titel", "Suche", "Abbrechen");
            if (promptResult.Ok)
            {
                MovieFilter = new Models.Filter()
                { Name = "Schnellfilter" };
                MovieFilter.FilterEntries.Add(
                    new FilterEntry() { FilterEntryType = new FilterEntryType() 
                    { Property = FilterEntryProperty.Title }, ValueFrom = promptResult.Text, Operator= FilterEntryOperator.Contains });
                await LoadMovies();
            }
        });

        private Movie CreateNewMovie()
        {
            Movie movie = Movie.CreateNew<Movie>();
            movie.LastSeen = DateTime.Today;
            movie.ReleaseDate = DateTime.Today;
            return movie;
        }

        public MoviesViewModel(IMovieService moviesService)
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
            viewModel.MovieDetails = Resolver.Resolve<IMovieService>().GetMovieDetails(miViewModel.Movie);
            viewModel.Title = "Filmdetails";

            await Navigation.PushAsync(detailView);
        }

        private async Task LoadMovies()
        {
            try
            {
                IsBusy = true;
                var movies = await MoviesService.GetWithGenresAndSourcesAsync(MovieSortOrder);
                Movies = new ObservableCollection<MoviesItemViewModel>(movies.Select(m => CreateMoviesItemViewModel(m)));

                if (MovieFilter == null)
                {
                    Title = "Filme";
                    MovieFilter = Resolver.Resolve<IFilterService>().GetDefault();
                }

                if (MovieFilter != null)
                {
                    Title = MovieFilter.Name;
                    try
                    {
                        Movies = new ObservableCollection<MoviesItemViewModel>(Movies.AsQueryable().Where(FilterBuilder<MoviesItemViewModel>.Ref.BuildFilter(MovieFilter)));
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetCurrentClassLogger().Error(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private MoviesItemViewModel CreateMoviesItemViewModel(Movie movie)
        {
            var moviesItemViewModel = new MoviesItemViewModel(movie);
            return moviesItemViewModel;
        }
    }
}