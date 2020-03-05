using Moviekus.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Moviekus.ViewModels
{
    // ViewModel für ein Element aus der ListView mit den Movies
    public class MoviesItemViewModel : BaseViewModel
    {
        public MoviesItemViewModel(Movie movie) => Movie = movie;

        public Movie Movie { get; set; }

        //public event EventHandler MovieStatusChanged;

    }
}
