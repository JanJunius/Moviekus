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

        public FilterEntryDataTemplateSelector()
        {
            FilterEntryEntryCellTemplate = new DataTemplate(typeof(FilterEntryEntryCellTemplate));
            FilterEntryDateCellTemplate = new DataTemplate(typeof(FilterEntryDateCellTemplate));
            FilterEntrySourceCellTemplate = new DataTemplate(typeof(FilterEntrySourceCellTemplate));
            FilterEntryGenreCellTemplate = new DataTemplate(typeof(FilterEntryGenreCellTemplate));
            FilterEntryRatingCellTemplate = new DataTemplate(typeof(FilterEntryRatingCellTemplate));
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
                default:
                    return FilterEntryEntryCellTemplate;
            }
            
        }
    }
}
