using Moviekus.Services;
using Moviekus.Views.Filter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
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

        public Models.Filter SelectedItem
        {
            get { return null; }
            set
            {
                if (value != null)
                {
                    Device.BeginInvokeOnMainThread(async () => await OpenEditPage(value));
                    RaisePropertyChanged(nameof(Filter));
                }
            }
        }

        private async Task OpenEditPage(Models.Filter filter)
        {
            var detailView = Resolver.Resolve<FilterDetailPage>();
            var viewModel = detailView.BindingContext as FilterDetailViewModel;
            viewModel.Filter = filter;
            viewModel.Title = "Filter bearbeiten";

            await Navigation.PushAsync(detailView);
        }

    }
}
