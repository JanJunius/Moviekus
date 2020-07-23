using Acr.UserDialogs;
using Moviekus.Dto;
using Moviekus.Dto.MovieDb;
using Moviekus.Models;
using Moviekus.ServiceContracts;
using Moviekus.Services;
using Moviekus.ViewModels.Genres;
using Moviekus.Views.Genres;
using Moviekus.Views.Movies;
using Moviekus.Views.Validation;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Moviekus.ViewModels.Movies
{
    public class MovieEditViewModel : BaseViewModel
    {
        public delegate void MovieChanged(object sender, Movie movie);
        public event MovieChanged OnMovieChanged;

        private Movie _movie;
        public Movie Movie 
        {
            get { return _movie; }
            set
            {
                _movie = value;
                if (_movie.Source != null)
                {
                    Source source = Sources.Where(s => s.Id == _movie.Source.Id).FirstOrDefault();
                    SelectedSource = source;
                }
            }
        }

        public string Genres
        {
            get
            {
                string genres = string.Empty;
                if (Movie != null)
                {
                    var genreList = Movie.MovieGenres.Where(mg => !mg.IsDeleted).Select(g => g.Genre);
                    genreList.ForEach(g => genres += g.Name + "; ");
                }
                return genres.Length > 1 ? genres.Substring(0, genres.Length - 2) : genres;
            }
            set { }
        }

        public ICommand SaveCommand => new Command(async () =>
        {
            if (!Validate())
            {
                await UserDialogs.Instance.AlertAsync(new AlertConfig
                {
                    Title = "Eingaben unvollständig",
                    Message = "Die Eingaben sind nicht korrekt. Bitte die markierten Stellen korrigieren."
                });
                return;
            }
            Movie = await MovieService.SaveMovieAsync(Movie);
            OnMovieChanged?.Invoke(this, Movie);
            await Navigation.PopAsync();
        });

        public override async void OnViewDisappearing()
        {
            if (!Validate())
            {
                // Zurücksetzen aller Änderungen wenn Eingaben ungültig und Page verlassen wird
                Movie origionalMovie = await Resolver.Resolve<IMovieService>().GetWithGenresAndSourcesAsync(Movie.Id);
                if (origionalMovie != null)
                {
                    Movie = origionalMovie;
                    OnMovieChanged?.Invoke(this, Movie);
                }
                
            }
            base.OnViewDisappearing();
        }

        public ICommand MovieDbCommand => new Command(async () =>
        {
            if (string.IsNullOrEmpty(Movie.Title) || Movie.Title.Length < 2)
            {
                await UserDialogs.Instance.AlertAsync(new AlertConfig
                {
                    Title = "MovieDb durchsuchen",
                    Message = "Bitte einen Titel mit mindestens 2 Zeichen eingeben."
                });
                return;
            }

            await OpenSelectionPage(MovieDbService.Ref);
        });

        public ICommand CoverButtonCommand => new Command(async () =>
        {
            Stream stream = await DependencyService.Get<IPhotoPickerService>().GetImageStreamAsync();
            if (stream != null)
            {
                Movie.Cover = Utils.GetImageBytes(stream);
                OnMovieChanged?.Invoke(this, Movie);
                RaisePropertyChanged(nameof(Movie));
            }
        });

        public bool Validate()
        {
            return FormValidator.IsFormValid(Movie, Navigation.NavigationStack.Last());
        }

        private async Task OpenSelectionPage(IMovieProvider movieProvider)
        {
            var selectionView = Resolver.Resolve<MovieSelectionPage>();
            var viewModel = selectionView.BindingContext as MovieSelectionViewModel;
            viewModel.Title = "Filmauswahl";
            viewModel.Movie = Movie;
            viewModel.LoadCommand.Execute(movieProvider);
            viewModel.OnMovieSelectionChanged += async (movie) => await ApplyMovieSelection(movie);
            await Navigation.PushAsync(selectionView);
        }

        private async Task ApplyMovieSelection(MovieDbMovie movieDto)
        {
            Movie.Title = movieDto.Title;
            Movie.Description = movieDto.Overview;
            Movie.ReleaseDate = movieDto.ReleaseDate;
            Movie.LastSeen = MoviekusDefines.MinDate;
            Movie.Rating = 0;
            Movie.Runtime = movieDto.Runtime;
            Movie.Cover = movieDto.Cover;
            Movie.Homepage = movieDto.Homepage;
            Movie.Trailer = movieDto.TrailerUrl;
            Movie.MovieGenres = await MovieService.AddMovieGenres(Movie, movieDto.Genres);

//            Validate();

            OnMovieChanged?.Invoke(this, Movie);
            RaisePropertyChanged(nameof(Movie));
            RaisePropertyChanged(nameof(Genres));
        }

        public ICommand GenreEditButtonClicked => new Command(async () =>
        {
            var genreSelectionView = Resolver.Resolve<GenreSelectionPage>();
            var viewModel = genreSelectionView.BindingContext as GenreSelectionViewModel;
            viewModel.Movie = Movie;
            viewModel.LoadGenresCommand.Execute(null);
            viewModel.OnGenreSelectionChanged += OnGenreSelectionChanged;
            await Navigation.PushAsync(genreSelectionView);
        });

        private void OnGenreSelectionChanged(GenreSelection genreSelection)
        {
            RaisePropertyChanged(nameof(Genres));
        }

        private List<Source> _sources;
        private IMovieService MovieService;

        public IList<Source> Sources
        {
            get
            {
                return _sources;
            }
        }

        private Source _selectedSource;
        public Source SelectedSource
        {
            get { return _selectedSource; }
            set
            {
                _selectedSource = value;
                if (Movie != null)
                    Movie.Source = _selectedSource;
            }
        }

        public MovieEditViewModel(IMovieService movieService)
        {
            _sources = new List<Source>(Resolver.Resolve<ISourceService>().Get());

            MovieService = movieService;
            Movie = Movie.CreateNew<Movie>();
            Movie.Title = "Neuer Film";
        }

    }
}
