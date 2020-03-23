using Moviekus.Models;
using Moviekus.Views.Movies;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Moviekus.ViewModels.Movies
{
    public class MovieDetailViewModel : BaseViewModel
    {       
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
            var newMovieView = Resolver.Resolve<NewMoviePage>();
            var viewModel = newMovieView.BindingContext as NewMovieViewModel;
            viewModel.Movie = Movie;

            await Navigation.PushAsync(newMovieView);
        });

        public MovieDetailViewModel()
        {
        }
    }
}
