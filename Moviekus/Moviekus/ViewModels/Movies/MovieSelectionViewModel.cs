using Moviekus.Dto;
using Moviekus.Models;
using Moviekus.Services;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Moviekus.ViewModels.Movies
{
    public class MovieSelectionViewModel : BaseViewModel
    {
        public delegate void MovieSelectionChanged(MovieDto selectedMovie);
        public event MovieSelectionChanged OnMovieSelectionChanged;

        // Enthält die Suchkriterien
        public Movie Movie { get; set; }

        public bool IsLoading { get; set; }

        public ICommand LoadCommand => new Command(async () =>
        {
            if (Movie == null)
                return;

            IsLoading = true;

            try
            {
                var movies = await MovieDbService.Ref.SearchMovieAsync(Movie.Title);
                Movies = new ObservableCollection<MovieDto>();

                await Task.Run(() =>
                {
                    foreach (MovieDto movieDto in movies)
                    {
                        movieDto.Cover = MovieDbService.Ref.GetMovieCover(movieDto);
                        Device.BeginInvokeOnMainThread(() => Movies.Add(movieDto));
                    }
                });
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
            }
            finally
            {
                IsLoading = false;
            }
        });

        public ObservableCollection<MovieDto> Movies { get; set; }

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
                    Task t = Task.Run(() => MovieDbService.Ref.FillMovieDetails(value));
                    t.Wait();
                    t = Task.Run(() => MovieDbService.Ref.FillMovieTrailer(value));
                    t.Wait();
                }

                OnMovieSelectionChanged?.Invoke(dto);
                RaisePropertyChanged(nameof(SelectedItem));
                Navigation.PopAsync();
            }
        }
    }
}
