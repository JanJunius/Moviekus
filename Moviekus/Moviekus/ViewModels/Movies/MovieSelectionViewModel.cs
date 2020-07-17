using Acr.UserDialogs;
using Moviekus.Dto;
using Moviekus.Dto.MovieDb;
using Moviekus.Models;
using Moviekus.ServiceContracts;
using NLog;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Moviekus.ViewModels.Movies
{
    public class MovieSelectionViewModel : BaseViewModel
    {
        public delegate void MovieSelectionChanged(MovieDbMovie selectedMovie);
        public event MovieSelectionChanged OnMovieSelectionChanged;

        // Enthält die Suchkriterien
        public Movie Movie { get; set; }

        public bool IsLoading { get; set; }

        private IMovieProvider MovieProvider;

        public ICommand LoadCommand => new Command<IMovieProvider>(async (provider) =>
        {
            if (Movie == null)
                return;

            MovieProvider = provider;
            IsLoading = true;

            try
            {
                var movies = await MovieProvider.SearchMovieAsync(Movie.Title);
                Movies = new ObservableCollection<MovieDbMovie>();

                await Task.Run(() =>
                {
                    foreach (MovieDbMovie movieDto in movies)
                    {
                        movieDto.Cover = MovieProvider.GetMovieCover(movieDto);
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

        public ObservableCollection<MovieDbMovie> Movies { get; set; }

        public MovieDbMovie SelectedItem
        {
            get { return null; }
            set
            {
                if (value == null)
                    return;

                MovieDbMovie dto = value;

                // Details zum Film nachladen bei Auswahl
                if (!string.IsNullOrEmpty(value.ProviderMovieId))
                {
                    Task t = Task.Run(() => MovieProvider.FillMovieDetails(value));
                    t.Wait();
                    t = Task.Run(() => MovieProvider.FillMovieTrailer(value));
                    t.Wait();
                }

                OnMovieSelectionChanged?.Invoke(dto);
                RaisePropertyChanged(nameof(SelectedItem));
                Navigation.PopAsync();
            }
        }
    }
}
