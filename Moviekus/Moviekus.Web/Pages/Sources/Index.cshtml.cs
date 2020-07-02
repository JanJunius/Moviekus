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
    public class IndexModel : PageModel
    {
        private readonly Moviekus.EntityFramework.MoviekusDbContext _context;

        public IndexModel(Moviekus.EntityFramework.MoviekusDbContext context)
        {
            _context = context;
        }

        public IList<Source> Source { get;set; }

        public async Task OnGetAsync()
        {
            Source = await _context.Sources.ToListAsync();
        }
    }
}
