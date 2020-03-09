using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Moviekus.Models;
using Moviekus.Views.Movies;
using Moviekus.Views.Genres;
using Moviekus.Views.Sources;

namespace Moviekus.Views
{
    [DesignTimeVisible(false)]
    public partial class MainPage : MasterDetailPage
    {
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();
        public MainPage()
        {
            InitializeComponent();

            MasterBehavior = MasterBehavior.Popover;

            Detail = new NavigationPage(Resolver.Resolve<MoviesPage>());

            //MenuPages.Add((int)MenuItemType.Movies, (NavigationPage)Detail);
        }

        public async Task NavigateFromMenu(int id)
        {
            if (!MenuPages.ContainsKey(id))
            {
                switch (id)
                {
                    case (int)MenuItemType.Movies:
                        MenuPages.Add(id, new NavigationPage(Resolver.Resolve<MoviesPage>()));
                        break;
                    case (int)MenuItemType.Genres:
                        MenuPages.Add(id, new NavigationPage(Resolver.Resolve<GenresPage>()));
                        break;
                    case (int)MenuItemType.Sources:
                        MenuPages.Add(id, new NavigationPage(Resolver.Resolve<SourcesPage>()));
                        break;
                    case (int)MenuItemType.About:
                        MenuPages.Add(id, new NavigationPage(new AboutPage()));
                        break;
                }
            }

            var newPage = MenuPages[id];

            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;

                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(100);

                IsPresented = false;
            }
        }
    }
}