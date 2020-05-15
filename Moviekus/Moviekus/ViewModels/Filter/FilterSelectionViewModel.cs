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
    public class FilterSelectionViewModel : BaseViewModel
    {
        public EventHandler<Models.Filter> FilterSelected;

        public ObservableCollection<FilterItemViewModel> Filter { get; set; }

        private IService<Models.Filter> FilterService;

        public FilterSelectionViewModel(FilterService filterService)
        {
            FilterService = filterService;
            Filter = new ObservableCollection<FilterItemViewModel>();
        }

        public ICommand LoadFilterCommand => new Command(async () =>
        {
            var filter = await FilterService.GetAsync();
            Filter = new ObservableCollection<FilterItemViewModel>(filter?.Select(f => CreateFilterItemViewModel(f)).OrderBy(f => f.Filter.Name));

            Filter.Insert(0, CreateFilterItemViewModel(new Models.Filter()
            {
                Id = string.Empty,
                Name = "Kein Filter"
            }));
        });

        public FilterItemViewModel SelectedFilter
        {
            get { return null; }
            set
            {
                if (value != null)
                {
                    // null zurückgeben wenn der NullFilter gewählt wurde (also kein Filter ausgewählt werden soll)
                    FilterSelected?.Invoke(this, string.IsNullOrEmpty(value.Filter.Id)? null : value.Filter);
                    Navigation.PopAsync();
                }
                    
            }
        }

        private FilterItemViewModel CreateFilterItemViewModel(Models.Filter filter)
        {
            return new FilterItemViewModel(filter);
        }

    }
}
