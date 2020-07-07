using Moviekus.Models;
using Moviekus.ServiceContracts;
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

            if (string.IsNullOrEmpty(Filter.Id))
                return "Zurücksetzen des Filters";

            if (Filter.FilterEntries.Count < 1)
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
                    return GetStringText(filterEntry);
                case FilterEntryProperty.Genre:
                    return GetGenreText(filterEntry);
                case FilterEntryProperty.LastSeen:
                case FilterEntryProperty.ReleaseDate:
                    return GetDateText(filterEntry);
                case FilterEntryProperty.Rating:
                    return GetIntegerText(filterEntry, "Sterne");
                case FilterEntryProperty.Runtime:
                    return GetIntegerText(filterEntry, "Minuten"); 
                case FilterEntryProperty.Source:
                    return GetSourceText(filterEntry);
                default:
                    return string.Empty;
            }
        }

        private string GetStringText(FilterEntry filterEntry)
        {
            switch(filterEntry.Operator)
            {
                case FilterEntryOperator.Equal: return $" ist gleich '{filterEntry.ValueFrom}'";
                case FilterEntryOperator.NotEqual: return $" ist nicht gleich '{filterEntry.ValueFrom}'";
                case FilterEntryOperator.Contains: return $" enthält '{filterEntry.ValueFrom}'";
                default: return "Ungültiger Filter-Operator";
            }
        }

        private string GetDateText(FilterEntry filterEntry)
        {
            DateTime dateFrom = DateTime.Parse(filterEntry.ValueFrom);
            DateTime dateTo = MoviekusDefines.MinDate;
                
            if (dateFrom == MoviekusDefines.MinDate)
                return " ist nicht gesetzt";            

            switch (filterEntry.Operator)
            {
                case FilterEntryOperator.Equal: return $" ist am {dateFrom:d}";
                case FilterEntryOperator.NotEqual: return $" ist nicht am {dateFrom:d}";
                case FilterEntryOperator.Greater: return $" liegt nach dem {dateFrom:d}";
                case FilterEntryOperator.Lesser: return $" liegt vor dem {dateFrom:d}";
                case FilterEntryOperator.Between:
                    if (DateTime.TryParse(filterEntry.ValueTo, out dateTo))
                        return $" liegt zwischen dem {dateFrom:d} und dem {dateTo:d}";
                    else return "Ungültige Filterwerte";
                default: return "Ungültiger Filter-Operator";
            }
        }

        private string GetIntegerText(FilterEntry filterEntry, string units)
        {
            int from, to = 0;

            if (!int.TryParse(filterEntry.ValueFrom, out from))
                return "Ungültige Filterwerte";

            switch (filterEntry.Operator)
            {
                case FilterEntryOperator.Equal: return $" ist {from} {units}";
                case FilterEntryOperator.NotEqual: return $" ist nicht {from} {units}";
                case FilterEntryOperator.Greater: return $" ist größer als {from} {units}";
                case FilterEntryOperator.Lesser: return $" ist kleiner als {from} {units}";
                case FilterEntryOperator.Between:
                    if (int.TryParse(filterEntry.ValueTo, out to))
                        return $" liegt zwischen {from} und {to} {units}";
                    else return "Ungültige Filterwerte";
                default: return "Ungültiger Filter-Operator";
            }
        }


        private string GetGenreText(FilterEntry filterEntry)
        {
            IGenreService genreService = Resolver.Resolve<IGenreService>();

            var genre = genreService.Get(filterEntry.ValueFrom);
            string genreName = genre != null ? genre.Name : "unbekanntes Genre";

            switch (filterEntry.Operator)
            {
                case FilterEntryOperator.Equal: return $" ist '{genreName}'";
                case FilterEntryOperator.NotEqual: return $" ist nicht '{genreName}'";
                default: return "Ungültiger Filter-Operator";
            }
        }

        private string GetSourceText(FilterEntry filterEntry)
        {
            ISourceService sourceService = Resolver.Resolve<ISourceService>();

            var source = sourceService.Get(filterEntry.ValueFrom);
            string sourceName = source != null ? source.Name : "unbekannte Quelle";

            switch (filterEntry.Operator)
            {
                case FilterEntryOperator.Equal: return $" '{sourceName}'";
                case FilterEntryOperator.NotEqual: return $" nicht '{sourceName}'";
                default: return "Ungültiger Filter-Operator";
            }
        }
    }
}
