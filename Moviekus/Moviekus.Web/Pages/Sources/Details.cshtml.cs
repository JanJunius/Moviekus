using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moviekus.Models;
using Moviekus.ServiceContracts;

namespace Moviekus.Web.Pages.Sources
{
    public class DetailsModel : PageModel
    {
        private ISourceService SourceService;

        public DetailsModel(ISourceService sourceService)
        {
            SourceService = sourceService;
        }

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
    }
}
