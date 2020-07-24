using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moviekus.Models;
using Moviekus.ServiceContracts;

namespace Moviekus.Web.Pages.Genres
{
    public class EditModel : PageModel
    {
        private IGenreService GenreService;

        public EditModel(IGenreService genreService)
        {
            GenreService = genreService;
        }

        [BindProperty]
        public Genre Genre { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id != null)
                Genre = await GenreService.GetAsync(id);
            else Genre = GenreService.CreateGenre();

            if (Genre == null)
                return NotFound();

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            await GenreService.SaveChangesAsync(Genre);

            return RedirectToPage("./Index");
        }
    }
}
