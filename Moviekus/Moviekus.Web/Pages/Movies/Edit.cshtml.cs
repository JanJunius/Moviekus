using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moviekus.Models;
using Moviekus.ServiceContracts;

namespace Moviekus.Web.Pages.Movies
{
    public class EditModel : PageModel
    {
        private IMovieService MovieService;

        public EditModel(IMovieService movieService)
        {
            MovieService = movieService;
        }

        [BindProperty]
        public Movie Movie { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
                return NotFound();

            Movie = await MovieService.GetWithGenresAndSourcesAsync(id);

            if (Movie == null)
                return NotFound();

            return Page();
        }
    }
}