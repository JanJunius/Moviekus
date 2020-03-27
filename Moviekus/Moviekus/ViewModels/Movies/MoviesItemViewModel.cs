using Moviekus.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Moviekus.ViewModels.Movies
{
    // ViewModel für ein Element aus der ListView mit den Movies
    public class MoviesItemViewModel : BaseViewModel
    {
        public MoviesItemViewModel(Movie movie) => Movie = movie;

        public Movie Movie { get; set; }

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
