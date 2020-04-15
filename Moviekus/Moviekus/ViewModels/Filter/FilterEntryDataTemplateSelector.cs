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
        private DataTemplate FilterEntryPickerCellTemplate;
        private DataTemplate FilterEntryRatingCellTemplate;

        public FilterEntryDataTemplateSelector()
        {
            FilterEntryEntryCellTemplate = new DataTemplate(typeof(FilterEntryEntryCellTemplate));
            FilterEntryDateCellTemplate = new DataTemplate(typeof(FilterEntryDateCellTemplate));
            FilterEntryPickerCellTemplate = new DataTemplate(typeof(FilterEntryPickerCellTemplate));
            FilterEntryRatingCellTemplate = new DataTemplate(typeof(FilterEntryRatingCellTemplate));
        }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var viewModel = item as FilterDetailItemViewModel;
            if (viewModel == null)
                return null;

            if (viewModel.FilterEntry.FilterEntryProperty == Models.FilterEntryProperty.LastSeen
                || viewModel.FilterEntry.FilterEntryProperty == Models.FilterEntryProperty.ReleaseDate)
                return FilterEntryDateCellTemplate;

            if (viewModel.FilterEntry.FilterEntryProperty == Models.FilterEntryProperty.Source
                || viewModel.FilterEntry.FilterEntryProperty == Models.FilterEntryProperty.Genre)
                return FilterEntryPickerCellTemplate;

            if (viewModel.FilterEntry.FilterEntryProperty == Models.FilterEntryProperty.Rating)
                return FilterEntryRatingCellTemplate;

            return FilterEntryEntryCellTemplate;
        }
    }
}
