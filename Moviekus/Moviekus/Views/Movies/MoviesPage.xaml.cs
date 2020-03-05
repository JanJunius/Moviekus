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
        private readonly MoviesViewModel viewModel;

        public MoviesPage(MoviesViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = viewModel;
            viewModel.Navigation = Navigation;

            this.viewModel = viewModel;
        }


        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(Resolver.Resolve<NewMoviePage>()));
        }

    }
}