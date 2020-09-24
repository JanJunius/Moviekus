using Moviekus.Models;
using Moviekus.ViewModels.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Moviekus.ViewModels.Filter
{
    public class FilterBuilder
    {
        private static FilterBuilder TheInstance;

        public static FilterBuilder Ref
        {
            get
            {
                if (TheInstance == null)
                    TheInstance = new FilterBuilder();
                return TheInstance;
            }
        }

        private FilterBuilder()
        {            
        }

        public Expression<Func<MoviesItemViewModel, bool>> BuildFilter(Models.Filter filter)
        {
            var predicates = new List<Expression<Func<MoviesItemViewModel, bool>>>();

            if (filter.FilterEntries.Any(v => v.FilterEntryType.Property == FilterEntryProperty.Title))
                predicates.Add(BuildTitleFilter(filter.FilterEntries.Where(v => v.FilterEntryType.Property == FilterEntryProperty.Title)));
            if (filter.FilterEntries.Any(v => v.FilterEntryType.Property == FilterEntryProperty.Remarks))
                predicates.Add(BuildRemarkFilter(filter.FilterEntries.Where(v => v.FilterEntryType.Property == FilterEntryProperty.Remarks)));
            if (filter.FilterEntries.Any(v => v.FilterEntryType.Property == FilterEntryProperty.Description))
                predicates.Add(BuildDescriptionFilter(filter.FilterEntries.Where(v => v.FilterEntryType.Property == FilterEntryProperty.Description)));
            if (filter.FilterEntries.Any(v => v.FilterEntryType.Property == FilterEntryProperty.ReleaseDate))
                predicates.Add(BuildReleaseDateFilter(filter.FilterEntries.Where(v => v.FilterEntryType.Property == FilterEntryProperty.ReleaseDate)));
            if (filter.FilterEntries.Any(v => v.FilterEntryType.Property == FilterEntryProperty.LastSeen))
                predicates.Add(BuildLastSeenFilter(filter.FilterEntries.Where(v => v.FilterEntryType.Property == FilterEntryProperty.LastSeen)));
            if (filter.FilterEntries.Any(v => v.FilterEntryType.Property == FilterEntryProperty.Rating))
                predicates.Add(BuildRatingFilter(filter.FilterEntries.Where(v => v.FilterEntryType.Property == FilterEntryProperty.Rating)));
            if (filter.FilterEntries.Any(v => v.FilterEntryType.Property == FilterEntryProperty.Runtime))
                predicates.Add(BuildRuntimeFilter(filter.FilterEntries.Where(v => v.FilterEntryType.Property == FilterEntryProperty.Runtime)));
            if (filter.FilterEntries.Any(v => v.FilterEntryType.Property == FilterEntryProperty.Source))
                predicates.Add(BuildSourceFilter(filter.FilterEntries.Where(v => v.FilterEntryType.Property == FilterEntryProperty.Source)));
            if (filter.FilterEntries.Any(v => v.FilterEntryType.Property == FilterEntryProperty.Genre))
                predicates.Add(BuildGenreFilter(filter.FilterEntries.Where(v => v.FilterEntryType.Property == FilterEntryProperty.Genre)));

            if (predicates.Count == 0)
                PredicateBuilder.False<MoviesItemViewModel>();

            var predicate = predicates.First();

            if (predicates.Count == 1)
                return predicate;

            foreach (var p in predicates.Where(p => p != predicate))
            {
                predicate = predicate.And(p);
            }

            return predicate;
        }

        private Expression<Func<MoviesItemViewModel, bool>> BuildTitleFilter(IEnumerable<FilterEntry> filterEntries)
        {
            var predicate = PredicateBuilder.False<MoviesItemViewModel>();

            foreach(var filterEntry in filterEntries)
            {
                string temp = filterEntry.ValueFrom.ToUpper();
                switch (filterEntry.Operator)
                {
                    case FilterEntryOperator.Contains: predicate = predicate.Or(p => p.Movie.Title.ToUpper().Contains(temp)); break;
                    case FilterEntryOperator.Equal: predicate = predicate.Or(p => p.Movie.Title.Equals(temp)); break;
                    case FilterEntryOperator.NotEqual: predicate = predicate.Or(p => !p.Movie.Title.Equals(temp)); break;
                    default: throw new InvalidFilterException($"Der Operator '{filterEntry.Operator}' ist für Titel nicht zulässig!");
                }
            }
            return predicate;
        }

        private Expression<Func<MoviesItemViewModel, bool>> BuildRemarkFilter(IEnumerable<FilterEntry> filterEntries)
        {
            var predicate = PredicateBuilder.False<MoviesItemViewModel>();

            foreach (var filterEntry in filterEntries)
            {
                string temp = filterEntry.ValueFrom;
                switch (filterEntry.Operator)
                {
                    case FilterEntryOperator.Contains: predicate = predicate.Or(p => !string.IsNullOrEmpty(p.Movie.Remarks) && p.Movie.Remarks.Contains(temp)); break;
                    case FilterEntryOperator.Equal: predicate = predicate.Or(p => !string.IsNullOrEmpty(p.Movie.Remarks) && p.Movie.Remarks.Equals(temp)); break;
                    case FilterEntryOperator.NotEqual: predicate = predicate.Or(p => !string.IsNullOrEmpty(p.Movie.Remarks) && !p.Movie.Remarks.Equals(temp)); break;
                    default: throw new InvalidFilterException($"Der Operator '{filterEntry.Operator}' ist für Bemerkungen nicht zulässig!");
                }
            }

            return predicate;
        }

        private Expression<Func<MoviesItemViewModel, bool>> BuildDescriptionFilter(IEnumerable<FilterEntry> filterEntries)
        {
            var predicate = PredicateBuilder.False<MoviesItemViewModel>();

            foreach (var filterEntry in filterEntries)
            {
                string temp = filterEntry.ValueFrom;
                switch (filterEntry.Operator)
                {
                    case FilterEntryOperator.Contains: predicate = predicate.Or(p => !string.IsNullOrEmpty(p.Movie.Description) && p.Movie.Description.Contains(temp)); break;
                    case FilterEntryOperator.Equal: predicate = predicate.Or(p => !string.IsNullOrEmpty(p.Movie.Description) && p.Movie.Description.Equals(temp)); break;
                    case FilterEntryOperator.NotEqual: predicate = predicate.Or(p => !string.IsNullOrEmpty(p.Movie.Description) && !p.Movie.Description.Equals(temp)); break;
                    default: throw new InvalidFilterException($"Der Operator '{filterEntry.Operator}' ist für Beschreibungen nicht zulässig!");
                }
            }

            return predicate;
        }

        private Expression<Func<MoviesItemViewModel, bool>> BuildReleaseDateFilter(IEnumerable<FilterEntry> filterEntries)
        {
            var predicate = PredicateBuilder.False<MoviesItemViewModel>();
            foreach (var filterEntry in filterEntries)
            {
                FilterEntry temp = filterEntry;
                DateTime dateFrom = DateTime.Parse(temp.ValueFrom);
                DateTime dateTo = MoviekusDefines.MinDate;

                switch(filterEntry.Operator)
                {
                    case FilterEntryOperator.Equal: predicate = predicate.Or(p => p.Movie.ReleaseDate.Equals(dateFrom)); break;
                    case FilterEntryOperator.NotEqual: predicate = predicate.Or(p => !p.Movie.ReleaseDate.Equals(dateFrom)); break;
                    case FilterEntryOperator.Greater: predicate = predicate.Or(p => p.Movie.ReleaseDate > dateFrom); break;
                    case FilterEntryOperator.Lesser: predicate = predicate.Or(p => p.Movie.ReleaseDate < dateFrom); break;
                    case FilterEntryOperator.Between:
                        if (DateTime.TryParse(temp.ValueTo, out dateTo))
                            predicate = predicate.Or(p => p.Movie.ReleaseDate >= dateFrom && p.Movie.ReleaseDate <= dateTo);
                        else throw new InvalidFilterException("Unvollständige Werte ist für das Veröffentlichungsdatum !");
                        break;
                    default: throw new InvalidFilterException($"Der Operator '{filterEntry.Operator}' ist für das Veröffentlichungsdatum nicht zulässig!");
                }
            }
            return predicate;
        }

        private Expression<Func<MoviesItemViewModel, bool>> BuildLastSeenFilter(IEnumerable<FilterEntry> filterEntries)
        {
            var predicate = PredicateBuilder.False<MoviesItemViewModel>();
            foreach (var filterEntry in filterEntries)
            {
                FilterEntry temp = filterEntry;
                DateTime dateFrom = DateTime.Parse(temp.ValueFrom);
                DateTime dateTo = MoviekusDefines.MinDate;

                switch (filterEntry.Operator)
                {
                    case FilterEntryOperator.Equal: predicate = predicate.Or(p => p.Movie.LastSeen.Equals(dateFrom)); break;
                    case FilterEntryOperator.NotEqual: predicate = predicate.Or(p => !p.Movie.LastSeen.Equals(dateFrom)); break;
                    case FilterEntryOperator.Greater: predicate = predicate.Or(p => p.Movie.LastSeen > dateFrom); break;
                    case FilterEntryOperator.Lesser: predicate = predicate.Or(p => p.Movie.LastSeen < dateFrom); break;
                    case FilterEntryOperator.Between: 
                        if (DateTime.TryParse(temp.ValueTo, out dateTo))
                            predicate = predicate.Or(p => p.Movie.LastSeen >= dateFrom && p.Movie.LastSeen <= dateTo);
                        else throw new InvalidFilterException("Unvollständige Werte ist für 'Zuletzt gesehen' !"); 
                        break;
                    default: throw new InvalidFilterException($"Der Operator '{filterEntry.Operator}' ist für 'Zuletzt gesehen' nicht zulässig!");
                }

            }
            return predicate;
        }

        private Expression<Func<MoviesItemViewModel, bool>> BuildRatingFilter(IEnumerable<FilterEntry> filterEntries)
        {
            var predicate = PredicateBuilder.False<MoviesItemViewModel>();
            foreach (var filterEntry in filterEntries)
            {
                FilterEntry temp = filterEntry;
                int ratingFrom = temp.ValueFrom != null ? int.Parse(temp.ValueFrom) : 0;
                int ratingTo = !string.IsNullOrEmpty(temp.ValueTo) ? int.Parse(temp.ValueTo) : 0;

                switch (filterEntry.Operator)
                {
                    case FilterEntryOperator.Equal: predicate = predicate.Or(p => p.Movie.Rating == ratingFrom); break;
                    case FilterEntryOperator.NotEqual: predicate = predicate.Or(p => p.Movie.Rating != ratingFrom); break;
                    case FilterEntryOperator.Greater: predicate = predicate.Or(p => p.Movie.Rating > ratingFrom); break;
                    case FilterEntryOperator.Lesser: predicate = predicate.Or(p => p.Movie.Rating < ratingFrom); break;
                    case FilterEntryOperator.Between: predicate = predicate.Or(p => p.Movie.Rating >= ratingFrom && p.Movie.Rating <= ratingTo); break;
                    default: throw new InvalidFilterException($"Der Operator '{filterEntry.Operator}' ist für die Bewertung nicht zulässig!");
                }

            }
            return predicate;
        }

        private Expression<Func<MoviesItemViewModel, bool>> BuildRuntimeFilter(IEnumerable<FilterEntry> filterEntries)
        {
            var predicate = PredicateBuilder.False<MoviesItemViewModel>();
            foreach (var filterEntry in filterEntries)
            {
                FilterEntry temp = filterEntry;
                int runtimeFrom;
                if (!int.TryParse(temp.ValueFrom, out runtimeFrom))
                    runtimeFrom = -1;
                int runtimeTo;
                if (!int.TryParse(temp.ValueTo, out runtimeTo))
                    runtimeTo = -1;

                switch (filterEntry.Operator)
                {
                    case FilterEntryOperator.Equal: predicate = predicate.Or(p => p.Movie.Runtime == runtimeFrom); break;
                    case FilterEntryOperator.NotEqual: predicate = predicate.Or(p => p.Movie.Runtime != runtimeFrom); break;
                    case FilterEntryOperator.Greater: predicate = predicate.Or(p => p.Movie.Runtime > runtimeFrom); break;
                    case FilterEntryOperator.Lesser: predicate = predicate.Or(p => p.Movie.Runtime < runtimeFrom); break;
                    case FilterEntryOperator.Between: predicate = predicate.Or(p => p.Movie.Runtime >= runtimeFrom && p.Movie.Runtime <= runtimeTo); break;
                    default: throw new InvalidFilterException($"Der Operator '{filterEntry.Operator}' ist für die Laufzeit nicht zulässig!");
                }

            }
            return predicate;
        }

        private Expression<Func<MoviesItemViewModel, bool>> BuildSourceFilter(IEnumerable<FilterEntry> filterEntries)
        {
            var predicate = PredicateBuilder.False<MoviesItemViewModel>();

            foreach (var filterEntry in filterEntries)
            {
                string sourceId = filterEntry.ValueFrom;
                switch (filterEntry.Operator)
                {
                    case FilterEntryOperator.Equal: predicate = predicate.Or(p => p.Movie.Source != null && p.Movie.Source.Id == sourceId); break;
                    case FilterEntryOperator.NotEqual: predicate = predicate.Or(p => p.Movie.Source != null && p.Movie.Source.Id != sourceId); break;
                    default: throw new InvalidFilterException($"Der Operator '{filterEntry.Operator}' ist für die Quelle nicht zulässig!");
                }
            }
                
            return predicate;
        }

        private Expression<Func<MoviesItemViewModel, bool>> BuildGenreFilter(IEnumerable<FilterEntry> filterEntries)
        {
            var predicate = PredicateBuilder.False<MoviesItemViewModel>();
            foreach (var filterEntry in filterEntries)
            {
                string genreId = filterEntry.ValueFrom;
                switch (filterEntry.Operator)
                {
                    case FilterEntryOperator.Equal: predicate = predicate.Or(p => p.Movie.MovieGenres.Any(b => b.Genre.Id == genreId)); break;
                    case FilterEntryOperator.NotEqual: predicate = predicate.Or(p => p.Movie.MovieGenres.Count(b => b.Genre.Id == genreId)==0); break;
                    default: throw new InvalidFilterException($"Der Operator '{filterEntry.Operator}' ist für das Genre nicht zulässig!");
                }
            }

            return predicate;
        }

    }
}
