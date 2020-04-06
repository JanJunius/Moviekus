using Moviekus.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Moviekus.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        private SettingsViewModel ViewModel;

        public SettingsPage(SettingsViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = ViewModel = viewModel;
            viewModel.Navigation = Navigation;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ViewModel.LoadSettingsCommand.Execute(null);
        }

    }
}