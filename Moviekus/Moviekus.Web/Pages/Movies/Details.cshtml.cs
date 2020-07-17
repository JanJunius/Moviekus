using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moviekus.Dto;
using Moviekus.Models;
using Moviekus.ServiceContracts;
using Moviekus.ViewModels.Movies;

namespace Moviekus.Web.Pages.Movies
{
    public class DetailsModel : PageModel
    {
        private IMovieService MovieService;

        public MovieDetailDto MovieDetails { get; set; }

        public DetailsModel(IMovieService movieService)
        {
            MovieService = movieService;
            MovieDetails = new MovieDetailDto();
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
                return NotFound();

            var movie = await MovieService.GetWithGenresAndSourcesAsync(id);
            if (movie == null)
                return NotFound();

            MovieDetails.Movie = movie;

            return Page();
        }

    }
}