using Moviekus.Models;

namespace Moviekus.ViewModels
{
    public class NewMovieViewModel : BaseViewModel
    {
        public Movie Movie { get; set; }

        public NewMovieViewModel()
        {
            Movie = new Movie()
            {
                Text = "Neu",
                Description = "Beschreibung"
            };
        }
    }
}
