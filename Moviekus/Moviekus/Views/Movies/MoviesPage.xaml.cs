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

            // Zurücksetzen der Selektion eines Films, damit man ihn direkt nochmal selektieren kann
            // (andernfalls müsste man erst einen anderen Film wählen und dann wieder zurück)
            MoviesListView.ItemSelected += (sender, args) => MoviesListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //if (viewModel.Movies.Count == 0)
            viewModel.LoadMoviesCommand.Execute(null);

        }

    }
}