using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using Moviekus.Models;
using Moviekus.Views.Movies;
using Moviekus.Services;

namespace Moviekus.ViewModels
{
    public class MoviesViewModel : BaseViewModel
    {
        public IMovieService<Movie> DataStore => DependencyService.Get<IMovieService<Movie>>();

        public ObservableCollection<Movie> Movies { get; set; }
        public Command LoadMoviesCommand { get; set; }

        public MoviesViewModel()
        {
            Title = "Filme";
            Movies = new ObservableCollection<Movie>();
            LoadMoviesCommand = new Command(async () => await ExecuteLoadMoviesCommand());

            MessagingCenter.Subscribe<NewMoviePage, Movie>(this, "Neu", async (obj, movie) =>
            {
                var newMovie = movie as Movie;
                Movies.Add(newMovie);
                await DataStore.AddMovieAsync(newMovie);
            });
        }

        async Task ExecuteLoadMoviesCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Movies.Clear();
                var movies = await DataStore.GetMoviesAsync(true);
                foreach (var movie in movies)
                {
                    Movies.Add(movie);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}