using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Moviekus.Models;
using Moviekus.Views;
using Moviekus.ViewModels;

namespace Moviekus.Views.Movies
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MoviesPage : ContentPage
    {
        MoviesViewModel viewModel;

        public MoviesPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new MoviesViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Movie;
            if (item == null)
                return;

            await Navigation.PushAsync(new MovieDetailPage(new MovieDetailViewModel(item)));

            // Manually deselect item.
            MoviesListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewMoviePage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Movies.Count == 0)
                viewModel.LoadMoviesCommand.Execute(null);
        }
    }
}