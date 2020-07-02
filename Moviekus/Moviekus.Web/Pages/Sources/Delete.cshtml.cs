using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Moviekus.EntityFramework;
using Moviekus.Models;

namespace Moviekus.Web.Pages.Sources
{
    public class DeleteModel : PageModel
    {
        private readonly Moviekus.EntityFramework.MoviekusDbContext _context;

        public DeleteModel(Moviekus.EntityFramework.MoviekusDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Source Source { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Source = await _context.Sources.FirstOrDefaultAsync(m => m.Id == id);

            if (Source == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Source = await _context.Sources.FindAsync(id);

            if (Source != null)
            {
                _context.Sources.Remove(Source);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
