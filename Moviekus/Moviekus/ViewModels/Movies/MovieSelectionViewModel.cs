using Moviekus.Dto;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Moviekus.ViewModels.Movies
{
    public class MovieSelectionViewModel : BaseViewModel
    {
        public delegate void MovieSelectionChanged(MovieDto selectedMovie);
        public event MovieSelectionChanged OnMovieSelectionChanged;

        public IEnumerable<MovieDto> Movies { get; set; }

        public MovieDto SelectedItem
        {
            get { return null; }
            set
            {
                if (value == null)
                    return;

                OnMovieSelectionChanged?.Invoke(value);
                RaisePropertyChanged(nameof(SelectedItem));
                Navigation.PopAsync();
            }
        }

    }
}
