using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moviekus.ServiceContracts;
using Moviekus.Services;
using Moviekus.ViewModels.Movies;

namespace Moviekus.Web.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private MovieService MovieService = new MovieService();

        public IList<MoviesItemViewModel> Movies { get; set; }

        public async Task OnGetAsync()
        {
            //TODO: Sortierreihenfolge
            var movies = await MovieService.GetWithGenresAndSourcesAsync(MovieSortOrder.None);
            Movies = new List<MoviesItemViewModel>(movies.Select(m => new MoviesItemViewModel(m)));
        }
    }
}
