using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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

        [BindProperty(SupportsGet = true)]
        public MovieSortOrder MovieSortOrder { get; set; }

        public async Task OnGetAsync()
        {
            Filter movieFilter = null;

            // Ist gar kein Filter selektiert (erster Aufruf), prüfe auf Defaultfilter und nehme diesen ggf. als Vorauswahl
            if (SelectedFilterId == null)
            {
                var defaultFilter = FilterService.GetDefault();
                if (defaultFilter != null)
                    SelectedFilterId = defaultFilter.Id;
            }
            // Gewählten Filter als Default speichern
            if (!string.IsNullOrEmpty(SelectedFilterId))
            {
                await FilterService.SetDefault(SelectedFilterId);
                movieFilter = FilterService.GetDefault();
            }                
            // Defaultfilter zurücksetzen bei Auswahl von "Kein Filter"
            else await FilterService.ResetDefault();

            // Die Sortierung ist im Gegensatz zum Filter nicht in der Datenbank abgelegt
            // Stattdessen wird sie als SessionState solange vorgehalten, bis das Session-Timeout (default 20 Minuten) abgelaufen ist
            // Um SessionState verwenden zu können, muss dieser aktiviert werden, siehe Startup
            if (MovieSortOrder != MovieSortOrder.None)
                HttpContext.Session.SetInt32("MovieSortOrder", (int)MovieSortOrder);
            else
            {
                var sortOrder = HttpContext.Session.GetInt32("MovieSortOrder");
                if (sortOrder > 0)
                    MovieSortOrder = (MovieSortOrder)Enum.Parse(typeof(MovieSortOrder), sortOrder.ToString());
            }

            var movies = await MovieService.GetWithGenresAndSourcesAsync(MovieSortOrder);

            if (!string.IsNullOrEmpty(QuickSearch))
                Movies = new List<MoviesItemViewModel>(movies.Where(m => m.Title.Contains(QuickSearch)).Select(m => new MoviesItemViewModel(m)));
            else Movies = new List<MoviesItemViewModel>(movies.Select(m => new MoviesItemViewModel(m)));

            if (movieFilter != null)
                Movies = Movies.AsQueryable().Where(FilterBuilder.FilterBuilder<MoviesItemViewModel>.Ref.BuildFilter(movieFilter)).ToList();

            FilterList = new SelectList(await FilterService.GetAsync(), nameof(Filter.Id), nameof(Filter.Name));
        }

        // Handler Methods sind keine Überschreibungen von Methoden sondern folgen einer festen Namenskonvention:
        // HTTP-Verb mit vorangestelltem On, also wie hier z.B. OnPost
        // Die Methoden können, müssen aber nicht async sein. Falls sie es sind, ist das Suffix "Async" optional, d.h. OnPost und OnPostAsync ist das gleiche.
        // Damit OnPost überhaupt auggerufen wird, ist ein entsprechendes Formulat mit method="post" erforderlich
        //public void OnPost()
        //{
        //}
    }
}
