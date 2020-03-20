using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Moviekus.ViewModels.Movies;

namespace Moviekus.Views.Movies
{
    [DesignTimeVisible(false)]
    public partial class MoviesPage : ContentPage
    {
        private readonly MoviesViewModel viewModel;

        public MoviesPage(MoviesViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = viewModel;
            viewModel.Navigation = Navigation;

            this.viewModel = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Movies.Count == 0)
                viewModel.LoadMoviesCommand.Execute(null);

        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(Resolver.Resolve<NewMoviePage>()));
        }

    }
}