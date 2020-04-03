using Moviekus.Dto;
using Moviekus.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;

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

                MovieDto dto = value;

                // Details zum Film nachladen bei Auswahl
                if (!string.IsNullOrEmpty(value.MovieDbId))
                {
                    Task t = Task.Run(() => new MovieDbService().FillMovieDetails(value));
                    t.Wait();
                }

                OnMovieSelectionChanged?.Invoke(dto);
                RaisePropertyChanged(nameof(SelectedItem));
                Navigation.PopAsync();
            }
        }

    }
}
