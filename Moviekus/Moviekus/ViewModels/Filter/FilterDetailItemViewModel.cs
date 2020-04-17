using Moviekus.Models;
using Moviekus.Services;
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
                filterEntry.PropertyChanged += (sender, args) => { if (!filterEntry.IsNew) filterEntry.IsModified = true; };
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

        public int Rating
        {
            get
            {
                if (!string.IsNullOrEmpty(FilterEntry.ValueFrom))
                    return int.Parse(FilterEntry.ValueFrom);
                return 0;
            }
            set
            {
                FilterEntry.ValueFrom = value.ToString();
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
