using Acr.UserDialogs;
using Moviekus.Models;
using Moviekus.OneDrive;
using Moviekus.Services;
using Moviekus.Views;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Moviekus.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public Settings Settings { get; set; }

        private SettingsService SettingsService;

        public ICommand LoadSettingsCommand => new Command(async () =>
        {
            var settings = await SettingsService.GetAsync();
            if (settings.Count() < 1)
            {
                Settings = Settings.CreateNew<Settings>();
                await SettingsService.SaveChangesAsync(Settings);
            }
            else Settings = settings.First();

            RaisePropertyChanged(nameof(Settings));
        });

        public override async void OnViewDisappearing()
        {
            base.OnViewDisappearing();
            await SettingsService.SaveChangesAsync(Settings);
        }

        public ICommand OpenLogCommand => new Command(async () =>
        {
            var logViewerPage = Resolver.Resolve<LogViewerPage>();
            var viewModel = logViewerPage.BindingContext as LogViewerViewModel;
            viewModel.Title = "Logdatei";
            await Navigation.PushAsync(logViewerPage);
        });

        public ICommand ResetOneDriveVCommand => new Command(async () =>
        {
            await GraphClientManager.Ref.SignOut();
            await UserDialogs.Instance.AlertAsync(new AlertConfig
            {
                Title = "OneDrive-Verbindung",
                Message = "Die Verbindung zu OneDrive wurde zurückgesetzt."
            });
        });

        public SettingsViewModel(SettingsService settingsService)
        {
            SettingsService = settingsService;
        }
    }
}
