using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moviekus.Models;
using Moviekus.ServiceContracts;

namespace Moviekus.Web.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private IMovieService MovieService;
        private IFilterService FilterService;

        public IndexModel(IMovieService movieService, IFilterService filterService)
        {
            MovieService = movieService;
            FilterService = filterService;
        }

        public IList<MoviesItemViewModel> Movies { get; set; }

        [BindProperty(SupportsGet = true)]
        public string QuickSearch { get; set; }

        public SelectList FilterList { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SelectedFilterId { get; set; }

        public async Task OnGetAsync()
        {
            Filter movieFilter = null;

            if (!string.IsNullOrEmpty(SelectedFilterId))
            {
                await FilterService.SetDefault(SelectedFilterId);
                movieFilter = FilterService.GetDefault();
            }                
            else await FilterService.ResetDefault();

            //TODO: Sortierreihenfolge
            var movies = await MovieService.GetWithGenresAndSourcesAsync(MovieSortOrder.None);

            if (!string.IsNullOrEmpty(QuickSearch))
                Movies = new List<MoviesItemViewModel>(movies.Where(m => m.Title.Contains(QuickSearch)).Select(m => new MoviesItemViewModel(m)));
            else Movies = new List<MoviesItemViewModel>(movies.Select(m => new MoviesItemViewModel(m)));

            if (movieFilter != null)
            {
                Movies = Movies.AsQueryable().Where(FilterBuilder.FilterBuilder<MoviesItemViewModel>.Ref.BuildFilter(movieFilter)).ToList();
            }

            FilterList = new SelectList(await FilterService.GetAsync(), nameof(Filter.Id), nameof(Filter.Name));
        }
    }
}
