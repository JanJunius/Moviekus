using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moviekus.Models;
using Moviekus.ServiceContracts;

namespace Moviekus.Web.Pages
{
    public class SettingsModel : PageModel
    {
        [BindProperty]
        public Settings Settings { get; set; }

        private ISettingsService SettingsService;

        public SettingsModel(ISettingsService settingsService)
        {
            SettingsService = settingsService;
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
                return Page();

            await SettingsService.SaveChangesAsync(Settings);

            return RedirectToPage("././Index");
        }
    }
}
