using Moviekus.Models;

namespace Moviekus.ViewModels.Movies
{
    public class MovieDetailViewModel : BaseViewModel
    {
        private Movie _movie;
        public Movie Movie 
        {
            get
            {
                return _movie;
            }
            set
            {
                _movie = value;
            }
        }
        
        public MovieDetailViewModel()
        {
        }
    }
}
