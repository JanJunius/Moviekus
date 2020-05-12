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
            //foreach (string title in titleEntries.Select(s => s.ValueFrom))
            //{
            //    string temp = title;

            //    predicate = predicate.Or(p => p.Movie.Title.Contains(temp));
            //}

            foreach(var filterEntry in filterEntries)
            {
                string temp = filterEntry.ValueFrom;
                switch (filterEntry.Operator)
                {
                    case FilterEntryOperator.Contains: predicate = predicate.Or(p => p.Movie.Title.Contains(temp)); break;
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
            //foreach (string remark in filterEntries.Select(s => s.ValueFrom))
            //{
            //    string temp = remark;
            //    predicate = predicate.Or(p => p.Movie.Remarks != null && p.Movie.Remarks.Contains(temp));
            //}

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
            //foreach (string remark in filterEntries.Select(s => s.ValueFrom))
            //{
            //    string temp = remark;
            //    predicate = predicate.Or(p => p.Movie.Description != null && p.Movie.Description.Contains(temp));
            //}

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
                DateTime dateTo = DateTime.Parse(temp.ValueTo);

                //if (dateFrom != MoviekusDefines.MinDate && dateTo == MoviekusDefines.MinDate)
                //    predicate = predicate.Or(p => p.Movie.ReleaseDate >= dateFrom);
                //else if (dateFrom == MoviekusDefines.MinDate && dateTo != MoviekusDefines.MinDate)
                //    predicate = predicate.Or(p => p.Movie.ReleaseDate <= dateFrom);
                //else predicate = predicate.Or(p => p.Movie.ReleaseDate >= dateFrom && p.Movie.ReleaseDate <= dateTo);

                switch(filterEntry.Operator)
                {
                    case FilterEntryOperator.Equal: predicate = predicate.Or(p => p.Movie.ReleaseDate.Equals(dateFrom)); break;
                    case FilterEntryOperator.NotEqual: predicate = predicate.Or(p => !p.Movie.ReleaseDate.Equals(dateFrom)); break;
                    case FilterEntryOperator.Greater: predicate = predicate.Or(p => p.Movie.ReleaseDate > dateFrom); break;
                    case FilterEntryOperator.Lesser: predicate = predicate.Or(p => p.Movie.ReleaseDate < dateFrom); break;
                    case FilterEntryOperator.Between: predicate = predicate.Or(p => p.Movie.ReleaseDate >= dateFrom && p.Movie.ReleaseDate <= dateTo); break;
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
                DateTime dateTo = DateTime.Parse(temp.ValueTo);

                //if (dateFrom != MoviekusDefines.MinDate && dateTo == MoviekusDefines.MinDate)
                //    predicate = predicate.Or(p => p.Movie.LastSeen >= dateFrom);
                //else if (dateFrom == MoviekusDefines.MinDate && dateTo != MoviekusDefines.MinDate)
                //    predicate = predicate.Or(p => p.Movie.LastSeen <= dateFrom);
                //else predicate = predicate.Or(p => p.Movie.LastSeen >= dateFrom && p.Movie.LastSeen <= dateTo);

                switch (filterEntry.Operator)
                {
                    case FilterEntryOperator.Equal: predicate = predicate.Or(p => p.Movie.LastSeen.Equals(dateFrom)); break;
                    case FilterEntryOperator.NotEqual: predicate = predicate.Or(p => !p.Movie.LastSeen.Equals(dateFrom)); break;
                    case FilterEntryOperator.Greater: predicate = predicate.Or(p => p.Movie.LastSeen > dateFrom); break;
                    case FilterEntryOperator.Lesser: predicate = predicate.Or(p => p.Movie.LastSeen < dateFrom); break;
                    case FilterEntryOperator.Between: predicate = predicate.Or(p => p.Movie.LastSeen >= dateFrom && p.Movie.LastSeen <= dateTo); break;
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
                int ratingFrom = int.Parse(temp.ValueFrom);
                int ratingTo = int.Parse(temp.ValueTo);

                //if (ratingFrom != 0 && ratingTo == 0)
                //    predicate = predicate.Or(p => p.Movie.Rating >= ratingFrom);
                //else if (ratingFrom == 0 && ratingTo != 0)
                //    predicate = predicate.Or(p => p.Movie.Rating <= ratingFrom);
                //else predicate = predicate.Or(p => p.Movie.Rating >= ratingFrom && p.Movie.Rating <= ratingTo);

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

                //if (runtimeFrom != -1 && runtimeTo == -1)
                //    predicate = predicate.Or(p => p.Movie.Runtime >= runtimeFrom);
                //else if (runtimeFrom == -1 && runtimeTo != -1)
                //    predicate = predicate.Or(p => p.Movie.Runtime <= runtimeFrom);
                //else predicate = predicate.Or(p => p.Movie.Runtime >= runtimeFrom && p.Movie.Runtime <= runtimeTo);

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
            //foreach (string sourceId in filterEntries.Select(s => s.ValueFrom))
            //{
            //    string temp = sourceId;
            //    predicate = predicate.Or(p => p.Movie.Source.Id == temp);
            //}

            foreach (var filterEntry in filterEntries)
            {
                string sourceId = filterEntry.ValueFrom;
                switch (filterEntry.Operator)
                {
                    case FilterEntryOperator.Equal: predicate = predicate.Or(p => p.Movie.Source.Id == sourceId); break;
                    case FilterEntryOperator.NotEqual: predicate = predicate.Or(p => p.Movie.Source.Id != sourceId); break;
                    default: throw new InvalidFilterException($"Der Operator '{filterEntry.Operator}' ist für die Quelle nicht zulässig!");
                }
            }
                
            return predicate;
        }

        private Expression<Func<MoviesItemViewModel, bool>> BuildGenreFilter(IEnumerable<FilterEntry> filterEntries)
        {
            var predicate = PredicateBuilder.False<MoviesItemViewModel>();
            //foreach (string genreId in filterEntries.Select(s => s.ValueFrom))
            //{
            //    string temp = genreId;
            //    predicate = predicate.Or(p => p.Movie.MovieGenres.Any(b => b.Genre.Id==temp));
            //}

            foreach (var filterEntry in filterEntries)
            {
                string genreId = filterEntry.ValueFrom;
                switch (filterEntry.Operator)
                {
                    case FilterEntryOperator.Equal: predicate.Or(p => p.Movie.MovieGenres.Any(b => b.Genre.Id == genreId)); break;
                    case FilterEntryOperator.NotEqual: predicate.Or(p => p.Movie.MovieGenres.Any(b => b.Genre.Id != genreId)); break;
                    default: throw new InvalidFilterException($"Der Operator '{filterEntry.Operator}' ist für das Genre nicht zulässig!");
                }
            }

            return predicate;
        }

    }
}
