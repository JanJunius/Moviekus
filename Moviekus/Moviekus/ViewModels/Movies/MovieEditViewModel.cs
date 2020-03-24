using Moviekus.Models;
using Moviekus.Services;
using Moviekus.ViewModels.Sources;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

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

        public ICommand SaveCommand => new Command(async () =>
        {
            await MovieService.SaveMovieAsync(Movie);
            await Navigation.PopAsync();

            OnMovieChanged?.Invoke(this, Movie);
        });

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
