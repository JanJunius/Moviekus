using Moviekus.Models;
using Moviekus.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Moviekus.ViewModels.Filter
{
    [DebuggerDisplay("Property = {FilterEntry.FilterEntryType}")]
    public class FilterDetailItemViewModel : BaseViewModel
    {
        private FilterEntry filterEntry;

        public FilterEntry FilterEntry 
        { 
            get { return filterEntry; }
            set
            {
                filterEntry = value;
                filterEntry.PropertyChanged += (sender, args) => 
                { 
                    if (!filterEntry.IsNew && !filterEntry.IsDeleted 
                        && args.PropertyName != nameof(filterEntry.IsModified) && args.PropertyName != nameof(filterEntry.IsNew)
                        && args.PropertyName != nameof(filterEntry.IsDeleted)) 
                    filterEntry.IsModified = true; 
                };
            }
        }

        public DateTime DateFrom
        {
            get 
            {
                if (filterEntry == null || string.IsNullOrEmpty(filterEntry.ValueTo))
                    return MoviekusDefines.MinDate;
                DateTime dt;
                if (DateTime.TryParse(filterEntry.ValueFrom, out dt))
                    return dt;
                return MoviekusDefines.MinDate;
            }
            set
            {
                FilterEntry.ValueFrom = value.ToString("d");
            }
        }

        public DateTime DateTo
        {
            get 
            {
                if (filterEntry == null || string.IsNullOrEmpty(filterEntry.ValueTo))
                    return MoviekusDefines.MinDate;
                DateTime dt;
                if (DateTime.TryParse(filterEntry.ValueTo, out dt))
                    return dt;
                return MoviekusDefines.MinDate;
            }
            set
            {
                if (value != null)
                    FilterEntry.ValueTo = value.ToString("d");
            }
        }

        public IList<Source> Sources => new List<Source>(new SourceService().Get());

        public Source Source
        {
            get 
            { 
                if (!string.IsNullOrEmpty(FilterEntry.ValueFrom))
                    return new SourceService().Get(FilterEntry.ValueFrom);
                return null;
            }
            set
            {
                if (value != null)
                    FilterEntry.ValueFrom = value.Id;
            }
        }

        public IList<Genre> Genres => new List<Genre>(new GenreService().Get());

        public Genre Genre
        {
            get
            {
                if (!string.IsNullOrEmpty(FilterEntry.ValueFrom))
                    return new GenreService().Get(FilterEntry.ValueFrom);
                return null;
            }
            set
            {
                if (value != null)
                    FilterEntry.ValueFrom = value.Id;
            }
        }

        public int RatingFrom
        {
            get
            {
                if (!string.IsNullOrEmpty(FilterEntry.ValueFrom))
                    return int.Parse(FilterEntry.ValueFrom);
                return 1;
            }
            set
            {
                FilterEntry.ValueFrom = value.ToString();
            }
        }

        public int RatingTo
        {
            get
            {
                if (!string.IsNullOrEmpty(FilterEntry.ValueTo))
                    return int.Parse(FilterEntry.ValueTo);
                return 1;
            }
            set
            {
                FilterEntry.ValueTo = value.ToString();
            }
        }

        public override bool Equals(object obj)
        {
            return obj is FilterDetailItemViewModel model &&
                   EqualityComparer<FilterEntry>.Default.Equals(FilterEntry, model.FilterEntry);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FilterEntry);
        }
    }
}
