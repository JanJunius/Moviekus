using Moviekus.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Moviekus.ViewModels.Filter
{
    public class FilterDetailItemViewModel : BaseViewModel
    {
        private FilterEntry filterEntry;

        public FilterEntry FilterEntry 
        { 
            get { return filterEntry; }
            set
            {
                filterEntry = value;
                filterEntry.PropertyChanged += (sender, args) => { filterEntry.IsModified = true; };
            }
        }

        public DateTime DateFrom
        {
            get { return DateTime.Parse(filterEntry.ValueFrom); }
            set
            {
                FilterEntry.ValueFrom = value.ToString("D");
            }
        }

        public DateTime DateTo
        {
            get 
            {  
               if (filterEntry == null || string.IsNullOrEmpty(filterEntry.ValueTo))
                    return DateTime.MinValue;
                return DateTime.Parse(filterEntry.ValueTo); 
            }
            set
            {
                if (value != null && value != DateTime.MinValue)
                    FilterEntry.ValueTo = value.ToString("D");
            }
        }

        public string FilterEntryLabel
        {
            get
            {
                if (FilterEntry != null)
                {
                    switch (FilterEntry.FilterEntryProperty)
                    {
                        case FilterEntryProperty.Description: return "Beschreibung";
                        case FilterEntryProperty.LastSeen: return "Zuletzt gesehen";
                        case FilterEntryProperty.Notes: return "Bemerkungen";
                        case FilterEntryProperty.Rating: return "Bewertung";
                        case FilterEntryProperty.ReleaseDate: return "Veröffentlichung";
                        case FilterEntryProperty.Runtime: return "Laufzeit";
                        case FilterEntryProperty.Source: return "Quelle";
                        case FilterEntryProperty.Title: return "Titel";
                    }
                }

                return string.Empty;
            }
        }

    }
}
