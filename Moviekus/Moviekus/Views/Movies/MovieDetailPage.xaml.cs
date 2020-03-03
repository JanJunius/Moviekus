using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Moviekus.Models;
using Moviekus.ViewModels;

namespace Moviekus.Views.Movies
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MovieDetailPage : ContentPage
    {
        MovieDetailViewModel viewModel;

        public MovieDetailPage(MovieDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public MovieDetailPage()
        {
            InitializeComponent();

            var item = new Movie
            {
                Text = "Item 1",
                Description = "This is an item description."
            };

            viewModel = new MovieDetailViewModel(item);
            BindingContext = viewModel;
        }
    }
}