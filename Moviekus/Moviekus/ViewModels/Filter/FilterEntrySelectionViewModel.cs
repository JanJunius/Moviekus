using Moviekus.Models;
using Moviekus.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Moviekus.ViewModels.Filter
{
    public class FilterEntrySelectionViewModel : BaseViewModel
    {
        public event EventHandler<FilterEntryType> FilterEntryTypeSelected;

        public Models.Filter Filter { get; set; }

        public ObservableCollection<FilterEntryType> FilterEntryTypes { get; set; }

        public FilterEntryType FilterEntryType 
        { 
            get { return null; } 
            set
            {
                FilterEntryTypeSelected?.Invoke(this, value);
                Navigation.PopAsync();
            }
        }

        public ICommand LoadFilterEntryTypesCommand => new Command(async () =>
        {
            var filterEntryTypes = await new FilterService().GetFilterEntryTypesAsync();
            FilterEntryTypes = new ObservableCollection<FilterEntryType>(filterEntryTypes);

            /*
            // NUr die FilterEntryTypes anbieten, die noch nicht verwendet werden
            var availableFilterTypes = await new FilterService().GetFilterEntryTypesAsync();
            var usedFilterTypes = Filter.FilterEntries.Select(f => f.FilterEntryType);
            var unUsedFilterTypes = (from c1 in availableFilterTypes select c1).Except(from c2 in usedFilterTypes select c2);
            FilterEntryTypes = new ObservableCollection<FilterEntryType>(unUsedFilterTypes);
            */
        });
    }
}
