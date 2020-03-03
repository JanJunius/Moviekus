using System;

using Moviekus.Models;

namespace Moviekus.ViewModels
{
    public class MovieDetailViewModel : BaseViewModel
    {
        public Movie Item { get; set; }
        public MovieDetailViewModel(Movie item = null)
        {
            Title = item?.Text;
            Item = item;
        }
    }
}
