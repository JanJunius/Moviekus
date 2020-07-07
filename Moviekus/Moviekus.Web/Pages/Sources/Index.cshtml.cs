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
    public class IndexModel : PageModel
    {
        private SourceService SourceService = new SourceService();

        public IList<Source> Sources { get;set; }

        public async Task OnGetAsync()
        {
            Sources = await SourceService.GetAsync();
        }
    }
}
