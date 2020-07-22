using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
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
    public class EditModel : PageModel
    {
        private IMovieService MovieService;
        private IGenreService GenreService;
        private ISourceService SourceService;

        public EditModel(IMovieService movieService, IGenreService genreService, ISourceService sourceService)
        {
            MovieService = movieService;
            GenreService = genreService;
            SourceService = sourceService;
        }

        [BindProperty]
        public Movie Movie { get; set; }

        [BindProperty]
        public string[] SelectedGenreIds { get; set; }
        
        [BindProperty]
        public IFormFile Cover { get; set; }

        [BindProperty]
        public string SelectedSourceId { get; set; }

        public SelectList Genres { get; set; }

        public SelectList Sources { get; set; }

        [DataType(DataType.Date)]
        [BindProperty]
        public DateTime? LastSeen { get; set; }

        [DataType(DataType.Date)]
        [BindProperty]
        public DateTime? ReleaseDate { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
                return NotFound();

            Movie = await MovieService.GetWithGenresAndSourcesAsync(id);

            if (Movie == null)
                return NotFound();

            if (Movie.ReleaseDate != MoviekusDefines.MinDate)
                ReleaseDate = Movie.ReleaseDate;
            else ReleaseDate = null;

            if (Movie.LastSeen != MoviekusDefines.MinDate)
                LastSeen = Movie.LastSeen;
            else LastSeen = null;

            IList<Genre> genres = await GenreService.GetAsync();
            Genres = new SelectList(genres, nameof(Genre.Id), nameof(Genre.Name));
            SelectedGenreIds = Movie.MovieGenres.Select(m => m.Genre).Select(g => g.Id).ToArray();

            IList<Source> sources = await SourceService.GetAsync();
            Sources = new SelectList(sources, nameof(Source.Id), nameof(Source.Name));           
            if (Movie.Source != null)
                SelectedSourceId = Movie.Source.Id;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (!ModelState.IsValid)
                return Page();

            // Cover wird nur dann gesetzt, wenn ein Cover explizit ausgewählt wurde, d.h. es ist null,
            // wenn ein vorhandenes Cover unverändert bleibt
            if (Cover != null)
            {
                // Neu ausgewähltes Cover übernehmen
                using (var stream = new MemoryStream())
                {
                    await Cover.CopyToAsync(stream);
                    Movie.Cover = stream.ToArray();
                }
            }
            else
            {
                // Unverändertes Cover: Damit es beim Speichern nicht zurückgesetzt wird, muss es
                // neu geladen werden
                var savedMovie = await MovieService.GetAsync(id);
                Movie.Cover = savedMovie?.Cover;
            }

            // Umsetzen der Nullable-DateTimes auf unseren blöden MinValue
            if (ReleaseDate.HasValue)
                Movie.ReleaseDate = ReleaseDate.Value;
            else Movie.ReleaseDate = MoviekusDefines.MinDate;
            if (LastSeen.HasValue)
                Movie.LastSeen = LastSeen.Value;
            else Movie.LastSeen = MoviekusDefines.MinDate;

            if (!string.IsNullOrEmpty(SelectedSourceId))
                Movie.Source = await SourceService.GetAsync(SelectedSourceId);

            // Abgleich der neuen Genre-Selektion mit der bereits gespeicherten
            Movie.MovieGenres = await MovieService.SyncMovieGenres(Movie.Id, SelectedGenreIds);

            await MovieService.SaveChangesAsync(Movie);

            return RedirectToPage("./Index");
        }
    }
}
