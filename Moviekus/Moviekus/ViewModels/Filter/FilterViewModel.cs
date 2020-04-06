using Moviekus.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Moviekus.ViewModels.Filter
{
    public class FilterViewModel : BaseViewModel
    {
        public ObservableCollection<Models.Filter> Filter { get; set; }

        private IService<Models.Filter> FilterService;

        public FilterViewModel(FilterService filterService)
        {
            FilterService = filterService;
            Filter = new ObservableCollection<Models.Filter>();
        }

        public ICommand LoadFilterCommand => new Command(async () =>
        {
            Filter.Clear();
            var filter = await FilterService.GetAsync();
            Filter = new ObservableCollection<Models.Filter>(filter);
        });

    }
}
