using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moviekus.Models;
using Moviekus.Services;

namespace Moviekus.Web.Pages.Genres
{
    public class IndexModel : PageModel
    {
        private readonly Moviekus.EntityFramework.MoviekusDbContext _context;

        private GenreService GenreService = new GenreService();

        public IndexModel(Moviekus.EntityFramework.MoviekusDbContext context)
        {
            _context = context;
        }

        public IList<Genre> Genres { get;set; }

        public async Task OnGetAsync()
        {
            Genres = await GenreService.GetAsync();
        }
    }
}
