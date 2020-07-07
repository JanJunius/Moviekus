using Moviekus.Models;
using Moviekus.ServiceContracts;
using Moviekus.Views.Genres;
using NLog;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Moviekus.ViewModels.Genres
{
    public class GenresViewModel : BaseViewModel
    {
        private IGenreService GenreService;

        public ObservableCollection<GenresItemViewModel> Genres { get; set; }

        public ICommand LoadGenresCommand => new Command(async () =>
        {
            await LoadGenres();
        });

        public ICommand AddGenreCommand => new Command(async () =>
        {
            var genreDetailView = Resolver.Resolve<GenreDetailPage>();
            var viewModel = genreDetailView.BindingContext as GenreDetailViewModel;
            viewModel.Genre = Genre.CreateNew<Genre>();
            viewModel.Title = "Neues Genre";

            await Navigation.PushAsync(genreDetailView);
        });

        public GenresViewModel(IGenreService genreService)
        {
            Title = "Genres";
            Genres = new ObservableCollection<GenresItemViewModel>();
            GenreService = genreService;

            // Wiederspiegeln der Datenbankänderungen in der Liste
            genreService.OnModelInserted += (sender, genre) => Genres.Add(CreateGenresItemViewModel(genre));
            genreService.OnModelUpdated += async (sender, genre) => await LoadGenres();
            genreService.OnModelDeleted += (sender, genre) => Genres.Remove(CreateGenresItemViewModel(genre));
        }

        // Dient lediglich dazu, auf die Auswahl eines Genre zu reagieren
        // Angesteuert über Binding in der Page
        public GenresItemViewModel SelectedItem
        {
            get { return null; }
            set
            {
                if (value != null)
                {
                    Device.BeginInvokeOnMainThread(async () => await OpenDetailPage(value));
                    RaisePropertyChanged(nameof(SelectedItem));
                }
            }
        }

        private async Task OpenDetailPage(GenresItemViewModel giViewModel)
        {
            var detailView = Resolver.Resolve<GenreDetailPage>();
            var viewModel = detailView.BindingContext as GenreDetailViewModel;
            viewModel.Genre = giViewModel.Genre;
            viewModel.Title = "Genre bearbeiten";

            await Navigation.PushAsync(detailView);
        }

        private async Task LoadGenres()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Genres.Clear();
                var genres = await GenreService.GetAsync();

                var itemViewModels = genres.Select(m => CreateGenresItemViewModel(m)).OrderBy(g => g.Genre.Name);
                Genres = new ObservableCollection<GenresItemViewModel>(itemViewModels);
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

        private GenresItemViewModel CreateGenresItemViewModel(Genre genre)
        {
            var genresItemViewModel = new GenresItemViewModel(genre);
            return genresItemViewModel;
        }
    }
}
