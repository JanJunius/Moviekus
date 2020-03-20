using Moviekus.Models;

namespace Moviekus.ViewModels.Movies
{
    public class NewMovieViewModel : BaseViewModel
    {
        public Movie Movie { get; set; }

        public NewMovieViewModel()
        {
            Movie = new Movie()
            {
                Title = "Neuer Film"
            };
        }
    }
}
