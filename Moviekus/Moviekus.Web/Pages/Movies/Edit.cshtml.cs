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
using Moviekus.Dto.MovieDb;
using Moviekus.Models;
using Moviekus.ServiceContracts;
using Moviekus.Services;
using Newtonsoft.Json;

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
        public IFormFile SelectedCover { get; set; }

         public SelectList Genres { get; set; }

        // Die Auswahl wird übernommen in Movie.Source.Id: Dadurch wird sichergestellt, dass Movie.Source überhaupt angelegt wird. d.h.
        // in der Validierung und im onPost hat man ein Objekt Movie.Source, welches bis auf die Id nicht initialisiert ist
        // Um weitere Properties zu füllen, müssten hidden Fields aufgenommen werden
        // Würde man stattdessen an eine Property des PageModel binden, wäre Movie.Source im onPost und in der Validierung null, 
        // da es in keinem Binding verwendet wird
        public SelectList Sources { get; set; }

        [DataType(DataType.Date)]
        [BindProperty]
        public DateTime? LastSeen { get; set; }

        [DataType(DataType.Date)]
        [BindProperty]
        public DateTime? ReleaseDate { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id != null)
                Movie = await MovieService.GetWithGenresAndSourcesAsync(id);
            else Movie = MovieService.CreateMovie();

            if (Movie == null)
                return NotFound();

            await Init();

            return Page();
        }

        public async Task<IActionResult> OnGetMovieSelection(string id, string providerMovieId)
        {
            // Bereits bekannten Film laden
            Movie = await MovieService.GetWithGenresAndSourcesAsync(id);
            
            // Falls nicht bekannt, neuen Film anlegen
            if (Movie == null)
                Movie = MovieService.CreateMovie();

            // Übernahme der Daten aus MovieDb
            IMovieProvider movieProvider = MovieProviderFactory.CreateMovieProvider(MovieProviders.MovieDb);
            var movieDto = await movieProvider.GetMovieAsync(providerMovieId);
            Movie = await MovieService.ApplyDtoData(Movie, movieDto);

            await Init();

            // Speichern des Films nach der Auswahl: Eigentlich nicht nötig, aber wegen des "falschen" Cover-Handlings notwendig,
            // damit das ausgewählte Cover nicht verloren geht (es ist nicht gebunden, da byte-Array)
            // Siehe auch Kommentar bei SetCover
            await SaveChanesAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostSaveChangesAsync()
        {
            if (!await Validate())
                return Page();

            await SetCover(Movie.Id);
            await SaveChanesAsync();

            return RedirectToPage("./Index");
        }

        private async Task SaveChanesAsync()
        {
            // Umsetzen der Nullable-DateTimes auf unseren blöden MinValue
            if (ReleaseDate.HasValue)
                Movie.ReleaseDate = ReleaseDate.Value;
            else Movie.ReleaseDate = MoviekusDefines.MinDate;
            if (LastSeen.HasValue)
                Movie.LastSeen = LastSeen.Value;
            else Movie.LastSeen = MoviekusDefines.MinDate;

            if (!string.IsNullOrEmpty(Movie.Source?.Id))
                Movie.Source = await SourceService.GetAsync(Movie.Source.Id);

            // Abgleich der neuen Genre-Selektion mit der bereits gespeicherten
            Movie.MovieGenres = await MovieService.SyncMovieGenres(Movie.Id, SelectedGenreIds);

            await MovieService.SaveChangesAsync(Movie);
        }

        private async Task Init()
        {
            if (Movie.ReleaseDate != MoviekusDefines.MinDate)
                ReleaseDate = Movie.ReleaseDate;
            else ReleaseDate = null;

            if (Movie.LastSeen != MoviekusDefines.MinDate)
                LastSeen = Movie.LastSeen;
            else LastSeen = null;

            await InitGenres();
            await InitSources();
        }

        private async Task InitSources()
        {
            IList<Source> sources = await SourceService.GetAsync();
            Sources = new SelectList(sources, nameof(Source.Id), nameof(Source.Name));
            if (Movie.Source != null)
                Movie.Source.Id = Movie.Source.Id;
        }

        private async Task InitGenres()
        {
            IList<Genre> genres = await GenreService.GetAsync();
            Genres = new SelectList(genres, nameof(Genre.Id), nameof(Genre.Name));
            SelectedGenreIds = Movie.MovieGenres.Select(m => m.Genre).Select(g => g.Id).ToArray();
        }

        // Generell ist es in einer Web-Anwendung ungünstig, Covers in der Datenbank abzuspeichern, da diese nicht
        // direkt gebunden werden können und die Seite auch unnötig aufblasen
        // Besser ist es, die URL des Covers zu speichern und das eigentliche Bild dann nachladen zu lassen
        private async Task SetCover(string movieId)
        {
            // Cover wird nur dann gesetzt, wenn ein Cover explizit ausgewählt wurde, d.h. es ist null,
            // wenn ein vorhandenes Cover unverändert bleibt
            if (SelectedCover != null)
            {
                // Neu ausgewähltes Cover übernehmen
                using (var stream = new MemoryStream())
                {
                    await SelectedCover.CopyToAsync(stream);
                    Movie.Cover = stream.ToArray();
                }
            }
            else
            {
                // Unverändertes Cover: Damit es beim Speichern nicht zurückgesetzt wird, muss es neu geladen werden
                // Ursache: Cover ist nicht gebunden und damit im onPost null
                // Alternative: Cover als hidden Field binden
                var savedMovie = await MovieService.GetAsync(movieId);
                Movie.Cover = savedMovie?.Cover;
            }
        }

        private async Task<bool> Validate()
        {
            // Das Model der Source validiert auf Name
            // Der Name der Source wird aber nicht an OnPost übermittelt, da nicht direkt gebunden
            // Daher muss diese Validierung hier abgeschaltet werden
            ModelState.Remove("Movie.Source.Name");

            if (!ModelState.IsValid)
            {
                await InitSources();
                await InitGenres();

                return false;
            }
            return true;
        }

        public IActionResult OnPostMovieSelection()
        {
            return RedirectToPage("./MovieSelection", new { id = Movie.Id, title = Movie.Title, MovieProviders.MovieDb });
        }
    }
}
