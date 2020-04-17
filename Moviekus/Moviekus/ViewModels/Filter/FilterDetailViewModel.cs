using Acr.UserDialogs;
using Moviekus.Dto;
using Moviekus.Models;
using Moviekus.Services;
using Moviekus.Views.Filter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Moviekus.ViewModels.Filter
{
    public class FilterDetailViewModel : BaseViewModel
    {
        private IService<Models.Filter> FilterService;

        private Models.Filter _Filter;
        public Models.Filter Filter 
        {
            get { return _Filter; }
            set
            {
                _Filter = value;
                LoadFilterEntries();
            }
        }

        public FilterDetailViewModel(FilterService filterService)
        {
            FilterService = filterService;
            FilterEntries = new ObservableCollection<FilterDetailItemViewModel>();
        }

        public ObservableCollection<FilterDetailItemViewModel> FilterEntries { get; set; }

        public FilterDetailItemViewModel SelectedFilterEntry 
        { 
            get; 
            set; 
        }

        public ICommand SaveCommand => new Command(async () =>
        {
            await FilterService.SaveChangesAsync(Filter);
            await Navigation.PopAsync();
        });

        public ICommand DeleteCommand => new Command(async () =>
        {
            var result = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig
            {
                Message = "Diesen Filter wirklich löschen?",
                OkText = "Ja",
                CancelText = "Nein"
            });
            if (result)
            {
                await FilterService.DeleteAsync(Filter);
                await Navigation.PopAsync();
            }
        });

        public ICommand AddEntryCommand => new Command(async () =>
        {
            var selectionView = Resolver.Resolve<FilterEntrySelectionPage>();
            var viewModel = selectionView.BindingContext as FilterEntrySelectionViewModel;

            viewModel.Title = "Filter hinzufügen";
            viewModel.LoadFilterEntryTypesCommand.Execute(null);

            FilterEntry filterEntry = AddFilterEntry();
            viewModel.FilterEntryTypeSelected += (sender, filterEntryType) => { filterEntry.FilterEntryType = filterEntryType; };

            await Navigation.PushAsync(selectionView);
        });

        public ICommand RemoveEntryCommand => new Command(() =>
        {
            RemoveFilterEntry();
        });

        private FilterEntry AddFilterEntry()
        {
            FilterEntry filterEntry = FilterEntry.CreateNew<FilterEntry>();
            filterEntry.Filter = Filter;
            Filter.FilterEntries.Add(filterEntry);
            FilterEntries.Add(CreateFilterDetailItemViewModel(filterEntry));
            return filterEntry;
        }

        private void RemoveFilterEntry()
        {
            SelectedFilterEntry.FilterEntry.IsDeleted = true;
            FilterEntries.Remove(CreateFilterDetailItemViewModel(SelectedFilterEntry.FilterEntry));
        }

        private void LoadFilterEntries()
        {
            FilterEntries.Clear();
            foreach(var filterEntry in Filter.FilterEntries)
            {
                FilterEntries.Add(CreateFilterDetailItemViewModel(filterEntry));
            }
        }

        private FilterDetailItemViewModel CreateFilterDetailItemViewModel(FilterEntry filterEntry)
        {
            var filterDetailItemViewModel = new FilterDetailItemViewModel()
            {
                FilterEntry = filterEntry
            };
            return filterDetailItemViewModel;
        }
    }
}
