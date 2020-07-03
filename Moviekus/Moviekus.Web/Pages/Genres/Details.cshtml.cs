using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moviekus.Models;
using Moviekus.Services;

namespace Moviekus.Web.Pages.Genres
{
    public class DetailsModel : PageModel
    {
        private readonly Moviekus.EntityFramework.MoviekusDbContext _context;

        private GenreService GenreService = new GenreService();

        public DetailsModel(Moviekus.EntityFramework.MoviekusDbContext context)
        {
            _context = context;
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
