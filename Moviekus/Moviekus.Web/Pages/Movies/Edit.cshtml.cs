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

        public string SelectedSourceId { get; set; }

        public SelectList Genres { get; set; }

        public SelectList Sources { get; set; }

        [DataType(DataType.Date)]
        public DateTime? LastSeen 
        { 
            get
            {
                if (Movie.LastSeen != MoviekusDefines.MinDate)
                    return Movie.LastSeen;
                else return null;
            }
            set
            {
                if (value != null && value.HasValue)
                    Movie.LastSeen = value.Value;
                else Movie.LastSeen = MoviekusDefines.MinDate;
            }
        }

        [DataType(DataType.Date)]
        public DateTime? ReleaseDate
        {
            get
            {
                if (Movie.ReleaseDate != MoviekusDefines.MinDate)
                    return Movie.ReleaseDate;
                else return null;
            }
            set
            {
                if (value != null && value.HasValue)
                    Movie.ReleaseDate = value.Value;
                else Movie.ReleaseDate = MoviekusDefines.MinDate;
            }
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
                return NotFound();

            Movie = await MovieService.GetWithGenresAndSourcesAsync(id);

            if (Movie == null)
                return NotFound();

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
            using (var stream = new MemoryStream())
            {
                await Cover.CopyToAsync(stream);
                Movie.Cover = stream.ToArray();
            }

            return Page();
        }
    }
}
