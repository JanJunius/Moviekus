using Moviekus.Models;
using System.Collections.Generic;

namespace Moviekus.ViewModels.Genres
{
    public class GenresItemViewModel : BaseViewModel
    {
        public Genre Genre { get; set; }

        public GenresItemViewModel(Genre genre)
        {
            Genre = genre;
        }

        public override bool Equals(object obj)
        {
            return obj is GenresItemViewModel model &&
                   EqualityComparer<Genre>.Default.Equals(Genre, model.Genre);
        }

        public override int GetHashCode()
        {
            return -898919840 + EqualityComparer<Genre>.Default.GetHashCode(Genre);
        }
    }
}
