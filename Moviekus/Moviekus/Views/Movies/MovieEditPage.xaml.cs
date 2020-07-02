using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Moviekus.Models;
using Moviekus.ViewModels;
using Moviekus.ViewModels.Movies;

namespace Moviekus.Views.Movies
{
    [DesignTimeVisible(false)]
    public partial class MovieEditPage : ContentPage
    {
        private MovieEditViewModel ViewModel;

        public MovieEditPage(MovieEditViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = ViewModel = viewModel;
            viewModel.Navigation = Navigation;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ViewModel.OnViewDisappearing();
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddItem", ViewModel.Movie);
            await Navigation.PopModalAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void Control_Unfocused(object sender, FocusEventArgs e)
        {
            ViewModel.Validate();
        }
    }
}