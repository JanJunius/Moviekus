using Moviekus.Models;
using Moviekus.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Moviekus.ViewModels.Filter
{
    public class FilterEntrySelectionViewModel : BaseViewModel
    {
        public event EventHandler<FilterEntryType> FilterEntryTypeSelected;

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
        });
    }
}
