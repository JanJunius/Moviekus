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
                predicates.Add(ContainsTitle(filter.FilterEntries.Where(v => v.FilterEntryType.Property == FilterEntryProperty.Title)));
            if (filter.FilterEntries.Any(v => v.FilterEntryType.Property == FilterEntryProperty.Remarks))
                predicates.Add(ContainsRemarks(filter.FilterEntries.Where(v => v.FilterEntryType.Property == FilterEntryProperty.Remarks)));
            if (filter.FilterEntries.Any(v => v.FilterEntryType.Property == FilterEntryProperty.Description))
                predicates.Add(ContainsDescription(filter.FilterEntries.Where(v => v.FilterEntryType.Property == FilterEntryProperty.Description)));
            if (filter.FilterEntries.Any(v => v.FilterEntryType.Property == FilterEntryProperty.ReleaseDate))
                predicates.Add(ReleaseDateBetween(filter.FilterEntries.Where(v => v.FilterEntryType.Property == FilterEntryProperty.ReleaseDate)));
            if (filter.FilterEntries.Any(v => v.FilterEntryType.Property == FilterEntryProperty.LastSeen))
                predicates.Add(LastSeenBetween(filter.FilterEntries.Where(v => v.FilterEntryType.Property == FilterEntryProperty.LastSeen)));
            if (filter.FilterEntries.Any(v => v.FilterEntryType.Property == FilterEntryProperty.Rating))
                predicates.Add(RatingBetween(filter.FilterEntries.Where(v => v.FilterEntryType.Property == FilterEntryProperty.Rating)));
            if (filter.FilterEntries.Any(v => v.FilterEntryType.Property == FilterEntryProperty.Runtime))
                predicates.Add(RuntimeBetween(filter.FilterEntries.Where(v => v.FilterEntryType.Property == FilterEntryProperty.Runtime)));
            if (filter.FilterEntries.Any(v => v.FilterEntryType.Property == FilterEntryProperty.Source))
                predicates.Add(HasSource(filter.FilterEntries.Where(v => v.FilterEntryType.Property == FilterEntryProperty.Source)));
            if (filter.FilterEntries.Any(v => v.FilterEntryType.Property == FilterEntryProperty.Genre))
                predicates.Add(HasGenre(filter.FilterEntries.Where(v => v.FilterEntryType.Property == FilterEntryProperty.Genre)));

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

        private Expression<Func<MoviesItemViewModel, bool>> ContainsTitle(IEnumerable<FilterEntry> titleEntries)
        {
            var predicate = PredicateBuilder.False<MoviesItemViewModel>();
            foreach (string title in titleEntries.Select(s => s.ValueFrom))
            {
                string temp = title;
                predicate = predicate.Or(p => p.Movie.Title.Contains(temp));
            }
            return predicate;
        }

        private Expression<Func<MoviesItemViewModel, bool>> ContainsRemarks(IEnumerable<FilterEntry> filterEntries)
        {
            var predicate = PredicateBuilder.False<MoviesItemViewModel>();
            foreach (string remark in filterEntries.Select(s => s.ValueFrom))
            {
                string temp = remark;
                predicate = predicate.Or(p => p.Movie.Remarks != null && p.Movie.Remarks.Contains(temp));
            }
            return predicate;
        }

        private Expression<Func<MoviesItemViewModel, bool>> ContainsDescription(IEnumerable<FilterEntry> filterEntries)
        {
            var predicate = PredicateBuilder.False<MoviesItemViewModel>();
            foreach (string remark in filterEntries.Select(s => s.ValueFrom))
            {
                string temp = remark;
                predicate = predicate.Or(p => p.Movie.Description != null && p.Movie.Description.Contains(temp));
            }
            return predicate;
        }

        private Expression<Func<MoviesItemViewModel, bool>> ReleaseDateBetween(IEnumerable<FilterEntry> filterEntries)
        {
            var predicate = PredicateBuilder.False<MoviesItemViewModel>();
            foreach (var filterEntry in filterEntries)
            {
                FilterEntry temp = filterEntry;
                DateTime dateFrom = DateTime.Parse(temp.ValueFrom);
                DateTime dateTo = DateTime.Parse(temp.ValueTo);

                if (dateFrom != MoviekusDefines.MinDate && dateTo == MoviekusDefines.MinDate)
                    predicate = predicate.Or(p => p.Movie.ReleaseDate >= dateFrom);
                else if (dateFrom == MoviekusDefines.MinDate && dateTo != MoviekusDefines.MinDate)
                    predicate = predicate.Or(p => p.Movie.ReleaseDate <= dateFrom);
                else predicate = predicate.Or(p => p.Movie.ReleaseDate >= dateFrom && p.Movie.ReleaseDate <= dateTo);
            }
            return predicate;
        }

        private Expression<Func<MoviesItemViewModel, bool>> LastSeenBetween(IEnumerable<FilterEntry> filterEntries)
        {
            var predicate = PredicateBuilder.False<MoviesItemViewModel>();
            foreach (var filterEntry in filterEntries)
            {
                FilterEntry temp = filterEntry;
                DateTime dateFrom = DateTime.Parse(temp.ValueFrom);
                DateTime dateTo = DateTime.Parse(temp.ValueTo);

                if (dateFrom != MoviekusDefines.MinDate && dateTo == MoviekusDefines.MinDate)
                    predicate = predicate.Or(p => p.Movie.LastSeen >= dateFrom);
                else if (dateFrom == MoviekusDefines.MinDate && dateTo != MoviekusDefines.MinDate)
                    predicate = predicate.Or(p => p.Movie.LastSeen <= dateFrom);
                else predicate = predicate.Or(p => p.Movie.LastSeen >= dateFrom && p.Movie.LastSeen <= dateTo);
            }
            return predicate;
        }

        private Expression<Func<MoviesItemViewModel, bool>> RatingBetween(IEnumerable<FilterEntry> filterEntries)
        {
            var predicate = PredicateBuilder.False<MoviesItemViewModel>();
            foreach (var filterEntry in filterEntries)
            {
                FilterEntry temp = filterEntry;
                int ratingFrom = int.Parse(temp.ValueFrom);
                int ratingTo = int.Parse(temp.ValueTo);

                if (ratingFrom != 0 && ratingTo == 0)
                    predicate = predicate.Or(p => p.Movie.Rating >= ratingFrom);
                else if (ratingFrom == 0 && ratingTo != 0)
                    predicate = predicate.Or(p => p.Movie.Rating <= ratingFrom);
                else predicate = predicate.Or(p => p.Movie.Rating >= ratingFrom && p.Movie.Rating <= ratingTo);
            }
            return predicate;
        }

        private Expression<Func<MoviesItemViewModel, bool>> RuntimeBetween(IEnumerable<FilterEntry> filterEntries)
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

                if (runtimeFrom != -1 && runtimeTo == -1)
                    predicate = predicate.Or(p => p.Movie.Runtime >= runtimeFrom);
                else if (runtimeFrom == -1 && runtimeTo != -1)
                    predicate = predicate.Or(p => p.Movie.Runtime <= runtimeFrom);
                else predicate = predicate.Or(p => p.Movie.Runtime >= runtimeFrom && p.Movie.Runtime <= runtimeTo);
            }
            return predicate;
        }

        private Expression<Func<MoviesItemViewModel, bool>> HasSource(IEnumerable<FilterEntry> filterEntries)
        {
            var predicate = PredicateBuilder.False<MoviesItemViewModel>();
            foreach (string sourceId in filterEntries.Select(s => s.ValueFrom))
            {
                string temp = sourceId;
                predicate = predicate.Or(p => p.Movie.Source.Id == temp);
            }
            return predicate;
        }

        private Expression<Func<MoviesItemViewModel, bool>> HasGenre(IEnumerable<FilterEntry> filterEntries)
        {
            var predicate = PredicateBuilder.False<MoviesItemViewModel>();
            foreach (string genreId in filterEntries.Select(s => s.ValueFrom))
            {
                string temp = genreId;
                predicate = predicate.Or(p => p.Movie.MovieGenres.Any(b => b.Genre.Id==temp));
            }
            return predicate;
        }

    }
}
