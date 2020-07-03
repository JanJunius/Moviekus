using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moviekus.Models;
using Moviekus.Services;

namespace Moviekus.Web.Pages.Genres
{
    public class EditModel : PageModel
    {
        private readonly Moviekus.EntityFramework.MoviekusDbContext _context;

        private GenreService GenreService = new GenreService();

        public EditModel(Moviekus.EntityFramework.MoviekusDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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
