using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moviekus.Dto.MovieDb;
using Moviekus.ServiceContracts;
using Moviekus.Services;
using Newtonsoft.Json;

namespace Moviekus.Web.Pages.Movies
{
    public class MovieSelectionModel : PageModel
    {
        [BindProperty]
        public string MovieId { get; set; }

        [BindProperty]
        public IList<MovieDbMovie> Movies { get; private set; }

        public async Task<IActionResult> OnGetAsync(string id, string title, MovieProviders provider)
        {
            MovieId = id;

            IMovieProvider movieProvider = MovieProviderFactory.CreateMovieProvider(provider);

            try
            {
                var movies = await movieProvider.SearchMovieAsync(title);
                Movies = new List<MovieDbMovie>();

                await Task.Run(() =>
                {
                    foreach (MovieDbMovie movieDto in movies)
                    {
                        movieDto.Cover = movieProvider.GetMovieCover(movieDto);
                        Movies.Add(movieDto);
                    }
                });
            }
            catch (Exception ex)
            {
                //TODO: Logging im WebProject
                //LogManager.GetCurrentClassLogger().Error(ex);
            }

            return Page();
        }
    }
}
