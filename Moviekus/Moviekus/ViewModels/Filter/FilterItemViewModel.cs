using Moviekus.Models;
using Moviekus.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moviekus.ViewModels.Filter
{
    public class FilterItemViewModel : BaseViewModel
    {
        public FilterItemViewModel(Models.Filter filter) => Filter = filter;

        public Models.Filter Filter { get; set;  }

        public override bool Equals(object obj)
        {
            return obj is FilterItemViewModel model &&
                   EqualityComparer<Models.Filter>.Default.Equals(Filter, model.Filter);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Filter);
        }

        public string Description
        {
            get { return BuildFilterDescription(); }
        }

        private string BuildFilterDescription()
        {
            string description = string.Empty;

            if (Filter == null || Filter.FilterEntries.Count < 1)
                return description;

            var query = from filterEntry in Filter.FilterEntries
                        group filterEntry by filterEntry.FilterEntryType into newGroup
                        select newGroup;

            foreach(var filterEntryType in query)
            {
                foreach(var filterValue in filterEntryType)
                {
                    description += filterEntryType.Key.Name;
                    description += GetValueText(filterValue);
                    description += " oder ";
                }
                description = description.Substring(0, description.Length - 6);
                description += System.Environment.NewLine + "und" + System.Environment.NewLine;
            }
            description = description.Substring(0, description.Length - 7);

            return description;
        }

        private string GetValueText(FilterEntry filterEntry)
        {
            switch(filterEntry.FilterEntryType.Property)
            {
                case FilterEntryProperty.Description:
                case FilterEntryProperty.Remarks:
                case FilterEntryProperty.Title:
                    return $" enthält '{filterEntry.ValueFrom}'";
                case FilterEntryProperty.Genre:
                    return GetGenreText(filterEntry);
                case FilterEntryProperty.LastSeen:
                case FilterEntryProperty.ReleaseDate:
                    return GetDateText(filterEntry);
                case FilterEntryProperty.Rating:
                    return GetRatingText(filterEntry);
                case FilterEntryProperty.Runtime:
                    return GetRuntimeText(filterEntry); ;
                case FilterEntryProperty.Source:
                    return GetSourceText(filterEntry);
                default:
                    return string.Empty;
            }
        }

        private string GetDateText(FilterEntry filterEntry)
        {
            DateTime dateFrom = DateTime.Parse(filterEntry.ValueFrom);
            DateTime dateTo = DateTime.Parse(filterEntry.ValueTo);

            if (dateFrom != MoviekusDefines.MinDate)
                return $" liegt zwischen dem {dateFrom:d} und dem {dateTo:d}";
            else return " ist nicht gesetzt";
                    
        }

        private string GetRuntimeText(FilterEntry filterEntry)
        {
            int from = int.Parse(filterEntry.ValueFrom);
            int to = 0;

            if (int.TryParse(filterEntry.ValueTo, out to))
            {
                if (int.TryParse(filterEntry.ValueFrom, out from))
                    return $" liegt zwischen {from} und {to} Minuten";
                else return $" ist kleiner als {to} Minuten";
            }
            
            if (int.TryParse(filterEntry.ValueFrom, out from))
                return $" ist größer als {from} Minuten";
            return string.Empty;
        }

        private string GetRatingText(FilterEntry filterEntry)
        {
            int from = int.Parse(filterEntry.ValueFrom);
            int to = 1;

            int.TryParse(filterEntry.ValueTo, out to);

            if (to > 1)
            {
                if (from > 1)
                    return $" hat zwischen {from} und {to} Sterne";
                else return $" ist kleiner als {to} Sterne";
            }
            return $" ist {from} Sterne";
        }

        private string GetGenreText(FilterEntry filterEntry)
        {
            GenreService genreService = new GenreService();
            return $" ist '{genreService.Get(filterEntry.ValueFrom).Name}'";
        }

        private string GetSourceText(FilterEntry filterEntry)
        {
            SourceService sourceService = new SourceService();
            return $" '{sourceService.Get(filterEntry.ValueFrom).Name}'";
        }
    }
}
