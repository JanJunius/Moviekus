using System;
using System.Collections.Generic;
using System.Text;

namespace Moviekus.Models
{
    public enum MenuItemType
    {
        Movies,
        Genres,
        Sources,
        About
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
