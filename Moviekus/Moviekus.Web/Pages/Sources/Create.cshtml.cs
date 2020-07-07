using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moviekus.EntityFramework;
using Moviekus.Models;
using Moviekus.Services;

namespace Moviekus.Web.Pages.Sources
{
    public class CreateModel : PageModel
    {
        private SourceService SourceService = new SourceService();

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Source Source { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            await SourceService.AddNewAsync(Source);

            return RedirectToPage("./Index");
        }
    }
}
