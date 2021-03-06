﻿using Moviekus.Dto;
using Moviekus.Models;
using Moviekus.Views.Filter;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Moviekus.ViewModels.Filter
{
    public class FilterEntryDataTemplateSelector : DataTemplateSelector
    {
        private DataTemplate FilterEntryEntryCellTemplate;
        private DataTemplate FilterEntryDateCellTemplate;
        private DataTemplate FilterEntrySourceCellTemplate;
        private DataTemplate FilterEntryGenreCellTemplate;
        private DataTemplate FilterEntryRatingCellTemplate;
        private DataTemplate FilterEntryRuntimeCellTemplate;

        public FilterEntryDataTemplateSelector()
        {
            FilterEntryEntryCellTemplate = new DataTemplate(typeof(FilterEntryEntryCellTemplate));
            FilterEntryDateCellTemplate = new DataTemplate(typeof(FilterEntryDateCellTemplate));
            FilterEntrySourceCellTemplate = new DataTemplate(typeof(FilterEntrySourceCellTemplate));
            FilterEntryGenreCellTemplate = new DataTemplate(typeof(FilterEntryGenreCellTemplate));
            FilterEntryRatingCellTemplate = new DataTemplate(typeof(FilterEntryRatingCellTemplate));
            FilterEntryRuntimeCellTemplate = new DataTemplate(typeof(FilterEntryRuntimeCellTemplate));
        }

        public static IList<FilterEntryOperator> GetAllowedOperators(FilterEntry filterEntry)
        {
            switch (filterEntry.FilterEntryType.Property)
            {
                case FilterEntryProperty.Title: 
                case FilterEntryProperty.Description:
                case FilterEntryProperty.Remarks: return new List<FilterEntryOperator>() { FilterEntryOperator.Equal, FilterEntryOperator.NotEqual, FilterEntryOperator.Contains };
                case FilterEntryProperty.Source:
                case FilterEntryProperty.Genre: return new List<FilterEntryOperator>() { FilterEntryOperator.Equal, FilterEntryOperator.NotEqual };
                case FilterEntryProperty.Rating:
                case FilterEntryProperty.Runtime:
                case FilterEntryProperty.ReleaseDate:
                case FilterEntryProperty.LastSeen: return new List<FilterEntryOperator>() { FilterEntryOperator.Equal, FilterEntryOperator.NotEqual, FilterEntryOperator.Between, FilterEntryOperator.Greater, FilterEntryOperator.Lesser }; 
                default: return new List<FilterEntryOperator>();
            }
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var viewModel = item as FilterDetailItemViewModel;
            if (viewModel == null)
                return null;

            switch(viewModel.FilterEntry.FilterEntryType.Property)
            {
                case Models.FilterEntryProperty.LastSeen:
                case Models.FilterEntryProperty.ReleaseDate:
                    return FilterEntryDateCellTemplate;
                case Models.FilterEntryProperty.Source:
                    return FilterEntrySourceCellTemplate;
                case Models.FilterEntryProperty.Genre:
                    return FilterEntryGenreCellTemplate;
                case Models.FilterEntryProperty.Rating:
                    return FilterEntryRatingCellTemplate;
                case Models.FilterEntryProperty.Runtime:
                    return FilterEntryRuntimeCellTemplate;
                default:
                    return FilterEntryEntryCellTemplate;
            }
            
        }
    }
}
