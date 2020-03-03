using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Moviekus.Services;
using Moviekus.Views;

namespace Moviekus
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockMovieService>();
            DependencyService.Register<MockSourceService>();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
