using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using Moviekus.EntityFramework;
using Moviekus.Models;
using Moviekus.Services;

namespace Moviekus.Web.Pages.Genres
{
    public class DeleteModel : PageModel
    {
        private GenreService GenreService = new GenreService();

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

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
                return NotFound();

            Genre = await GenreService.GetAsync(id);
            if (Genre != null)
                await GenreService.DeleteAsync(Genre);

            return RedirectToPage("./Index");
        }
    }
}
