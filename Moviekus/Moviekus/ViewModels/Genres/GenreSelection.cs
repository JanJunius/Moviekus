using Moviekus.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Moviekus.ViewModels.Genres
{
    public class GenreSelection 
    {
        public delegate void GenreSelectionChanged(GenreSelection genreSelection);
        public event GenreSelectionChanged OnGenreSelectionChanged;

        public Genre Genre { get; set; }

        private bool _selected;
        public bool Selected
        {
            get { return _selected; }
            set 
            { 
                _selected = value;
                OnGenreSelectionChanged?.Invoke(this);
            }
        }

    }
}