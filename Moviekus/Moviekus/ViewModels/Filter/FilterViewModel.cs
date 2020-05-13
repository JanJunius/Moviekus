using Moviekus.Services;
using Moviekus.Views.Filter;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;


namespace Moviekus.ViewModels.Filter
{
    public class FilterViewModel : BaseViewModel
    {
        public ObservableCollection<FilterItemViewModel> Filter { get; set; }

        private IService<Models.Filter> FilterService;

        public FilterViewModel(FilterService filterService)
        {
            FilterService = filterService;
            Filter = new ObservableCollection<FilterItemViewModel>();
            Title = "Filter";
        }

        public ICommand LoadFilterCommand => new Command(async () =>
        {
            var filter = await FilterService.GetAsync();
            Filter = new ObservableCollection<FilterItemViewModel>(filter.Select(f => CreateFilterItemViewModel(f)));
        });

        private FilterItemViewModel CreateFilterItemViewModel(Models.Filter filter)
        {
            return new FilterItemViewModel(filter);
        }

        public ICommand AddFilterCommand => new Command(async () => 
        {
            var filter = Models.Filter.CreateNew<Models.Filter>();
            filter.Name = "Neuer Filter";
            FilterItemViewModel viewModel = CreateFilterItemViewModel(filter);
            //Filter.Add(viewModel);
            await OpenEditPage(viewModel);
        });

        public FilterItemViewModel SelectedFilter
        {
            get { return null; }
            set
            {
                if (value != null)
                {
                    try
                    {
                        Device.BeginInvokeOnMainThread(async () => await OpenEditPage(value));
                        RaisePropertyChanged(nameof(Filter));

                    }
                    catch(Exception ex)
                    {
                        LogManager.GetCurrentClassLogger().Warn(ex);
                    }
                }
            }
        }

        private async Task OpenEditPage(FilterItemViewModel filterItemViewModel)
        {
            var detailView = Resolver.Resolve<FilterDetailPage>();
            var viewModel = detailView.BindingContext as FilterDetailViewModel;
            viewModel.Filter = filterItemViewModel.Filter;
            viewModel.Title = "Filter bearbeiten";
            viewModel.FilterChanged += (sender, changedFilter) => { LoadFilterCommand.Execute(null); };
            viewModel.FilterDeleted += (sender, deletedFilter) => { Filter.Remove(CreateFilterItemViewModel(deletedFilter)); };

            await Navigation.PushAsync(detailView);
        }

    }
}
