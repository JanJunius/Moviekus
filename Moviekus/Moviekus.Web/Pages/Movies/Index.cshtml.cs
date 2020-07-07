using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Moviekus.Models;
using Moviekus.Services;
using Moviekus.ViewModels.Movies;

namespace Moviekus.Web.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly Moviekus.EntityFramework.MoviekusDbContext _context;

        private MovieService MovieService = new MovieService();

        public IndexModel(Moviekus.EntityFramework.MoviekusDbContext context)
        {
            _context = context;
        }

        public IList<MoviesItemViewModel> Movies { get; set; }

        public async Task OnGetAsync()
        {
            //TODO: Sortierreihenfolge
            var movies = await MovieService.GetWithGenresAndSourcesAsync(MovieSortOrder.None);
            Movies = new List<MoviesItemViewModel>(movies.Select(m => new MoviesItemViewModel(m)));
        }
    }
}
