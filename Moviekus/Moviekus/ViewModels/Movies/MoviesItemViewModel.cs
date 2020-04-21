using Moviekus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms.Internals;

namespace Moviekus.ViewModels.Movies
{
    // ViewModel für ein Element aus der ListView mit den Movies
    public class MoviesItemViewModel : BaseViewModel
    {
        public MoviesItemViewModel(Movie movie) => Movie = movie;

        public Movie Movie { get; set; }

        public string SubTitle => BuildSubTitle();

        public override bool Equals(object obj)
        {
            return obj is MoviesItemViewModel model &&
                   EqualityComparer<Movie>.Default.Equals(Movie, model.Movie);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Movie);
        }

        private string BuildSubTitle()
        {
            string subTitle = string.Empty;

            if (Movie == null)
                return subTitle;

            string genres = string.Empty;
            if (Movie != null)
            {
                var genreList = Movie.MovieGenres.Select(g => g.Genre);
                genreList.ForEach(g => genres += g.Name + "; ");
            }
            if (genres.Length > 1)
                subTitle = genres.Substring(0, genres.Length - 2);

            if (Movie.Runtime > 0)
            {
                if (subTitle.Length > 0)
                    subTitle += ", ";
                subTitle += $"{Movie.Runtime} Min.";
            }
                
            if (Movie.ReleaseDate != MoviekusDefines.MinDate)
            {
                if (subTitle.Length > 0)
                    subTitle += ", ";
                subTitle += Movie.ReleaseDate.Year.ToString();
            }

            return subTitle;
        }
    }
}
