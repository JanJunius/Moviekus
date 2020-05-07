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
using NLog;
using Acr.UserDialogs;
using Moviekus.OneDrive;

namespace Moviekus.ViewModels.Movies
{
    public class MoviesViewModel : BaseViewModel
    {
        private MovieService MoviesService;

        private MovieSortOrder MovieSortOrder = MovieSortOrder.None;

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
            var actionResult = await UserDialogs.Instance.ActionSheetAsync("Nach was soll sortiert werden?", "Abbrechen", null, null, "Nichts", "Titel", "Laufzeit", "Bewertung", "Zuletzt gesehen", "Veröffentlichungsdatum");
            if (actionResult == "Nichts")
                MovieSortOrder = MovieSortOrder.None;
            else if (actionResult == "Titel")
                MovieSortOrder = MovieSortOrder.Title;
            if (actionResult == "Laufzeit")
                MovieSortOrder = MovieSortOrder.Runtime;
            if (actionResult == "Bewertung")
                MovieSortOrder = MovieSortOrder.Rating;
            if (actionResult == "Zuletzt gesehen")
                MovieSortOrder = MovieSortOrder.LastSeen;
            if (actionResult == "Veröffentlichungsdatum")
                MovieSortOrder = MovieSortOrder.ReleaseDate;
            await LoadMovies();
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
                var movies = await MoviesService.GetWithGenresAndSourcesAsync(MovieSortOrder);

                Movies = new ObservableCollection<MoviesItemViewModel>(movies.Select(m => CreateMoviesItemViewModel(m)));
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
            }
        }

        private MoviesItemViewModel CreateMoviesItemViewModel(Movie movie)
        {
            var moviesItemViewModel = new MoviesItemViewModel(movie);
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
                LogManager.GetCurrentClassLogger().Error(ex);
            }
        }

        private async Task RemoveFilter()
        {
            await LoadMovies();
        }

        private async Task OrderMovies(MovieSortOrder sortOrder)
        {
            switch(sortOrder)
            {
                case MovieSortOrder.Title:
                    Movies = new ObservableCollection<MoviesItemViewModel>(Movies.OrderBy(m => m.Title));
                    break;
            }
        }
    }
}