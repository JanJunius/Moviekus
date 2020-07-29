using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Moviekus.Models;
using Moviekus.ViewModels;
using Moviekus.ViewModels.Movies;
using Acr.UserDialogs;

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

            // Der Backbutton wird von Xamarin für Android nicht erwartungsgemäß behandelt: OnBackButtonPressed wird nicht gefeuert,
            // wenn man ihn antippt. Dann funktioniert die Validierung nicht => Abschalten
            if (Device.RuntimePlatform == Device.Android)
                NavigationPage.SetHasBackButton(this, false);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ViewModel.OnViewDisappearing();
        }

        protected override bool OnBackButtonPressed()
        {
            if (ViewModel.Validate())
            {
                Device.BeginInvokeOnMainThread(async () => await ViewModel.SaveChanges());
                return base.OnBackButtonPressed(); 
            }
            else
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var result = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig
                    {
                        Title = "Eingaben unvollständig",
                        Message = "Die Eingaben sind nicht korrekt. Möchten Sie die Maske schließen, ohne die Änderungen zu speichern?",
                        CancelText = "Ja, nicht speichern",
                        OkText = "Nein"
                    });
                    if (!result)
                    {
                        base.OnBackButtonPressed();
                        await ViewModel.UndoChanges();
                        await Navigation.PopAsync();
                    }
                });
            }

            return true;
        }


        private void Control_Unfocused(object sender, FocusEventArgs e)
        {
            ViewModel.Validate();
        }
    }
}