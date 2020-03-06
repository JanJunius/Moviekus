using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Moviekus.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public ICommand ClickCommand => new Command<string>((url) =>
        {
            Launcher.OpenAsync(new System.Uri(url));
        });

        public AboutViewModel()
        {
            Title = "Über Moviekus";
        }
    }
}