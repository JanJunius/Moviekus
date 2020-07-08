using Moviekus.Models;
using Moviekus.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moviekus.ViewModels.Movies
{
    public class MovieDetails
    {
        public Movie Movie { get; set; }

        public string Genres
        {
            get 
            {
                string genres = string.Empty;
                if (Movie != null)
                {
                    var genreList = Movie.MovieGenres.Select(g => g.Genre);
                    //genreList.ForEach(g => genres += g.Name + "; ");
                    foreach (Genre g in genreList)
                        genres += g.Name + "; ";
                }
                return genres.Length > 1 ? genres.Substring(0, genres.Length - 2) : genres;
            }
        }

        public string ReleaseDateText => Movie != null && Movie.ReleaseDate != MoviekusDefines.MinDate ? Movie.ReleaseDate.ToString("d", MoviekusDefines.MoviekusCultureInfo) : "<unbekannt>";

        public string LastSeenText => Movie != null && Movie.LastSeen != MoviekusDefines.MinDate ? Movie.LastSeen.ToString("d", MoviekusDefines.MoviekusCultureInfo) : "<noch nicht gesehen>";

        public bool HasHomepage => Movie != null ? !string.IsNullOrEmpty(Movie.Homepage) : false;
        public bool HasTrailer => Movie != null ? !string.IsNullOrEmpty(Movie.Trailer) : false;
    }
}
