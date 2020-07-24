using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moviekus.Models;
using Moviekus.ServiceContracts;

namespace Moviekus.Web.Pages.Sources
{
    public class IndexModel : PageModel
    {
        private ISourceService SourceService;

        public IndexModel(ISourceService sourceService)
        {
            SourceService = sourceService;
        }

        public IList<Source> Sources { get;set; }

        public async Task OnGetAsync()
        {
            Sources = await SourceService.GetAsync();
        }
    }
}
