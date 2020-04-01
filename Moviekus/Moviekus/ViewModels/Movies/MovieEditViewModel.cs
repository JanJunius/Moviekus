﻿using Acr.UserDialogs;
using Moviekus.Dto;
using Moviekus.Models;
using Moviekus.Services;
using Moviekus.ViewModels.Genres;
using Moviekus.ViewModels.Sources;
using Moviekus.Views;
using Moviekus.Views.Genres;
using Moviekus.Views.Movies;
using System.Collections.Generic;
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
                Source source = Sources.Where(s => s.Id == _movie.Source.Id).FirstOrDefault();
                SelectedSource = source;
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
            set { } // Setter notwendig fürs DataBinding
        }

        public ICommand SaveCommand => new Command(async () =>
        {
            await MovieService.SaveMovieAsync(Movie);
            await Navigation.PopAsync();

            OnMovieChanged?.Invoke(this, Movie);
        });

        public ICommand MovieDbCommand => new Command(async () =>
        {
            if (string.IsNullOrEmpty(Movie.Title) || Movie.Title.Length < 3)
            {
                await UserDialogs.Instance.AlertAsync(new AlertConfig
                {
                    Message = "Bitte einen Titel mit mindestens 3 Zeichen eingeben."
                });
                return;
            }

            IEnumerable<MovieDto> movieDtos = new MovieDbService().SearchMovie(Movie.Title);

            if (movieDtos.Count() == 0)
            {
                await UserDialogs.Instance.AlertAsync(new AlertConfig
                {
                    Message = "Es wurde kein passender Film gefunden."
                });                
            }
            // Wurde genau ein Film gefunden, dann übernehmen wir den
            else if (movieDtos.Count() == 1)
            {
                MovieDto movieDto = movieDtos.FirstOrDefault();
                ApplyMovieSelection(movieDto);
            }
            // Wurde mehr als ein Film gefunden, muss man einen auswählen
            else
            {
                var selectionView = Resolver.Resolve<MovieSelectionPage>();
                var viewModel = selectionView.BindingContext as MovieSelectionViewModel;
                viewModel.Title = "Filmauswahl";
                viewModel.Movies = movieDtos;
                viewModel.OnMovieSelectionChanged += async (movie) => await ApplyMovieSelection(movie); 
                await Navigation.PushAsync(selectionView);
            }
        });

        private async Task ApplyMovieSelection(MovieDto movieDto)
        {
            Movie.Title = movieDto.Title;
            Movie.Description = movieDto.Overview;
            Movie.ReleaseDate = movieDto.ReleaseDate;
            Movie.Runtime = movieDto.Runtime;
            Movie.MovieGenres = await MovieService.GetMovieGenres(Movie, movieDto.Genres);

            //OnMovieChanged?.Invoke(this, Movie);
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

        public MovieEditViewModel(MovieService movieService)
        {
            _sources = new List<Source>(new SourceService().Get());

            MovieService = movieService;
            Movie = Movie.CreateNew<Movie>();
            Movie.Title = "Neuer Film";
        }
    }
}
