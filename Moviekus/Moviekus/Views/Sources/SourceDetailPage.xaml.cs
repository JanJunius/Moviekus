using Acr.UserDialogs;
using Moviekus.ViewModels.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Moviekus.Views.Sources
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SourceDetailPage : ContentPage
    {
        private SourceDetailViewModel ViewModel;

        public SourceDetailPage(SourceDetailViewModel viewModel)
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

    }
}