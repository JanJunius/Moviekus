using Acr.UserDialogs;
using Moviekus.Models;
using Moviekus.Services;
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

        public ICommand SaveCommand => new Command(async () =>
        {
            await SettingsService.SaveChangesAsync(Settings);
            await UserDialogs.Instance.AlertAsync(new AlertConfig
            {
                Message = "Einstellungen wurden gespeichert."
            });
        });

        public SettingsViewModel(SettingsService settingsService)
        {
            SettingsService = settingsService;
        }
    }
}
