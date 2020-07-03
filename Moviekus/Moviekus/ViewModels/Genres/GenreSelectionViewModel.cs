using Moviekus.Models;
using Moviekus.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Moviekus.ViewModels.Genres
{
    public class GenreSelectionViewModel : BaseViewModel
    {
        public delegate void GenreSelectionChanged(GenreSelection genreSelection);
        public event GenreSelectionChanged OnGenreSelectionChanged;

        public ObservableCollection<GenreSelection> GenreSelection { get; set; }
        private BaseService<Genre> GenreService;

        public Movie Movie { get; set; }

        public GenreSelectionViewModel(GenreService genreService)
        {
            GenreService = genreService;
        }

        public ICommand LoadGenresCommand => new Command(async () =>
        {
            await LoadGenres();
        });

        private async Task LoadGenres()
        {
            var genres = await GenreService.GetAsync();

            var genreSelection = genres.Select(g => CreateGenreSelection(g)).OrderBy(g => g.Genre.Name);
            GenreSelection = new ObservableCollection<GenreSelection>(genreSelection);

            GenreSelection.ForEach(g => g.OnGenreSelectionChanged += GenreSelectionChangedHandler);
        }

        private GenreSelection CreateGenreSelection(Genre genre)
        {
            GenreSelection genreSelection = new GenreSelection()
            {
                Genre = genre,
                Selected = Movie != null ? Movie.MovieGenres.Any(mg => mg.Genre.Id == genre.Id && !mg.IsDeleted) : false
            };

            return genreSelection;
        }

        private void GenreSelectionChangedHandler(GenreSelection genreSelection)
        {
            MovieGenre movieGenre = Movie.MovieGenres.Where(mg => mg.Genre.Id == genreSelection.Genre.Id).FirstOrDefault();

            if (genreSelection.Selected)
            {
                if (movieGenre == null)
                {
                    movieGenre = MovieGenre.CreateNew<MovieGenre>();
                    movieGenre.Genre = genreSelection.Genre;
                    movieGenre.Movie = Movie;
                    Movie.MovieGenres.Add(movieGenre);
                    OnGenreSelectionChanged?.Invoke(genreSelection);
                }
                else if (movieGenre.IsDeleted)
                    movieGenre.IsDeleted = false;
            }
            else if (movieGenre != null)
            {
                // Wird ein Genre abgewählt, dass noch nicht gespeichert war, kann es einfach aus der Liste entfernt werden.
                // War es schon gespeichert, muss es auf Deleted gesetzt werden, damit das Delete in der DB stattfinden kann.
                if (movieGenre.IsNew)
                    Movie.MovieGenres.Remove(movieGenre);
                else movieGenre.IsDeleted = true;   
                OnGenreSelectionChanged?.Invoke(genreSelection);
            }
        }
    }
}
