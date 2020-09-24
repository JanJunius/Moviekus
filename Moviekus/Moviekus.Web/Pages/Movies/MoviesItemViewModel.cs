using Moviekus.FilterBuilder;
using Moviekus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviekus.Web.Pages.Movies
{
    public class MoviesItemViewModel : IFilterBuilderViewModel
    {
        public MoviesItemViewModel(Movie movie) => Movie = movie;

        public Movie Movie { get; set; }

        public string SubTitle => SubTitleBuilder.BuildSubTitle(Movie);

        public override bool Equals(object obj)
        {
            return obj is MoviesItemViewModel model &&
                   EqualityComparer<Movie>.Default.Equals(Movie, model.Movie);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Movie);
        }

    }
}
