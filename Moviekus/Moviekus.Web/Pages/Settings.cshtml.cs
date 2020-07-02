using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Moviekus.Models;
using Moviekus.Services;

namespace Moviekus.Web.Pages
{
    public class SettingsModel : PageModel
    {
        private readonly Moviekus.EntityFramework.MoviekusDbContext _context;

        [BindProperty]
        public Settings Settings { get; set; }

        private SettingsService SettingsService = new SettingsService();

        public SettingsModel(Moviekus.EntityFramework.MoviekusDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Settings = await SettingsService.GetSettingsAsync();

            if (Settings == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await SettingsService.SaveChangesAsync(Settings);

            return RedirectToPage("././Index");
        }

    }
}
