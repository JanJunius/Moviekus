using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Moviekus.EntityFramework;
using Moviekus.Models;

namespace Moviekus.Web.Pages.Sources
{
    public class EditModel : PageModel
    {
        private readonly Moviekus.EntityFramework.MoviekusDbContext _context;

        public EditModel(Moviekus.EntityFramework.MoviekusDbContext context)
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

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Source).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SourceExists(Source.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool SourceExists(string id)
        {
            return _context.Sources.Any(e => e.Id == id);
        }
    }
}
