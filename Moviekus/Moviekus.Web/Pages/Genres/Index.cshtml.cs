using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moviekus.Models;
using Moviekus.Services;

namespace Moviekus.Web.Pages.Genres
{
    public class IndexModel : PageModel
    {
        private GenreService GenreService = new GenreService();

        public IList<Genre> Genres { get;set; }

        public async Task OnGetAsync()
        {
            Genres = await GenreService.GetAsync();
        }
    }
}
