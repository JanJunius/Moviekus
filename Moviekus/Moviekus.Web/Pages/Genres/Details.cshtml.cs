using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moviekus.Models;
using Moviekus.ServiceContracts;

namespace Moviekus.Web.Pages.Genres
{
    public class DetailsModel : PageModel
    {
        private IGenreService GenreService;

        public DetailsModel(IGenreService genreService)
        {
            GenreService = genreService;
        }

        public Genre Genre { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
                return NotFound();

            Genre = await GenreService.GetAsync(id);

            if (Genre == null)
                return NotFound();

            return Page();
        }
    }
}
