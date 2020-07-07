using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Moviekus.EntityFramework;
using Moviekus.Models;
using Moviekus.Services;

namespace Moviekus.Web.Pages.Sources
{
    public class DeleteModel : PageModel
    {
        private SourceService SourceService = new SourceService();

        [BindProperty]
        public Source Source { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
                return NotFound();

            Source = await SourceService.GetAsync(id);

            if (Source == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
                return NotFound();

            Source = await SourceService.GetAsync(id);

            if (Source != null)
                await SourceService.DeleteAsync(Source);

            return RedirectToPage("./Index");
        }
    }
}
