using Moviekus.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Moviekus.ViewModels.Filter
{
    public class FilterSelectionViewModel : BaseViewModel
    {
        public EventHandler<Models.Filter> FilterSelected;

        public ObservableCollection<Models.Filter> Filter { get; set; }

        private IService<Models.Filter> FilterService;

        public FilterSelectionViewModel(FilterService filterService)
        {
            FilterService = filterService;
            Filter = new ObservableCollection<Models.Filter>();
        }

        public ICommand LoadFilterCommand => new Command(async () =>
        {
            Filter = new ObservableCollection<Models.Filter>(await FilterService.GetAsync());
            Filter.Insert(0, new Models.Filter()
            {
                Id = string.Empty,
                Name = "Kein Filter"
            });
        });

        public Models.Filter SelectedFilter
        {
            get { return null; }
            set
            {
                if (value != null)
                {
                    // null zurückgeben wenn der NullFilter gewählt wurde (also kein Filter ausgewählt werden soll)
                    FilterSelected?.Invoke(this, string.IsNullOrEmpty(value.Id)? null : value);
                    Navigation.PopAsync();
                }
                    
            }
        }

    }
}
