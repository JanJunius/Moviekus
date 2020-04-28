using Moviekus.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Moviekus.Views
{
    [DesignTimeVisible(false)]
    public partial class MenuPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        
        List<HomeMenuItem> menuItems;

        public MenuPage()
        {
            InitializeComponent();

            menuItems = new List<HomeMenuItem>
            {
                new HomeMenuItem {Id = MenuItemType.Movies, Title="Filme", ImageSource="movie.png" },
                new HomeMenuItem {Id = MenuItemType.Genres, Title="Genres", ImageSource="genres.png" },
                new HomeMenuItem {Id = MenuItemType.Sources, Title="Quellen", ImageSource="sources.png" },
                new HomeMenuItem {Id = MenuItemType.Filter, Title="Filter", ImageSource="filter.png" },
                new HomeMenuItem {Id = MenuItemType.Settings, Title="Einstellungen", ImageSource="settings.png" },
                new HomeMenuItem {Id = MenuItemType.About, Title="Über Moviekus", ImageSource="moviekus.png" }
            };

            ListViewMenu.ItemsSource = menuItems;
            ListViewMenu.SelectedItem = menuItems[0];
            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                var id = (int)((HomeMenuItem)e.SelectedItem).Id;
                await RootPage.NavigateFromMenu(id);
            };
        }
    }
}