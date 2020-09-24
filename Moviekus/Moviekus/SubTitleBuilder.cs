using Moviekus.Models;
using System.Linq;

namespace Moviekus
{
    public static class SubTitleBuilder
    {
        public static string BuildSubTitle(Movie movie)
        {
            string subTitle = string.Empty;

            if (movie == null)
                return subTitle;

            string genres = string.Empty;
            if (movie != null)
            {
                var genreList = movie.MovieGenres.Select(g => g.Genre);
                foreach (var genre in genreList)
                    genres += genre.Name + "; ";
            }
            if (genres.Length > 1)
                subTitle = genres.Substring(0, genres.Length - 2);

            if (movie.Runtime > 0)
            {
                if (subTitle.Length > 0)
                    subTitle += ", ";
                subTitle += $"{movie.Runtime} Min.";
            }

            if (movie.ReleaseDate != MoviekusDefines.MinDate)
            {
                if (subTitle.Length > 0)
                    subTitle += ", ";
                subTitle += movie.ReleaseDate.Year.ToString();
            }

            return subTitle;
        }
    }
}
