using System;
using System.Collections.Generic;
using System.Text;

namespace Moviekus
{
    public enum MenuItemType
    {
        Movies,
        Genres,
        Sources,
        Filter,
        Settings,
        About
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }

        public string ImageSource { get; set; }
    }
}
