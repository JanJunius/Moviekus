using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moviekus.Models;
using Moviekus.ServiceContracts;

namespace Moviekus.Web.Pages.Sources
{
    public class CreateModel : PageModel
    {
        private ISourceService SourceService;

        public CreateModel(ISourceService sourceService)
        {
            SourceService = sourceService;
        }

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
