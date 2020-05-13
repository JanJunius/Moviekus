using Acr.UserDialogs;
using Moviekus.Dto;
using Moviekus.Models;
using Moviekus.Services;
using Moviekus.Views.Filter;
using MvvmHelpers;
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
    public class FilterDetailViewModel : BaseViewModel
    {
        public EventHandler<Models.Filter> FilterChanged;
        public EventHandler<Models.Filter> FilterDeleted;

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
        }

        private readonly ObservableRangeCollection<Grouping<string, FilterDetailItemViewModel>> _filterEntries = new ObservableRangeCollection<Grouping<string, FilterDetailItemViewModel>>();
        public ObservableCollection<Grouping<string, FilterDetailItemViewModel>> FilterEntries => _filterEntries;

        public FilterDetailItemViewModel SelectedFilterEntry { get; set; }

        public ICommand SaveCommand => new Command(async () =>
        {
            try
            {
                await FilterService.SaveChangesAsync(Filter);
                await Navigation.PopAsync();
                FilterChanged?.Invoke(this, Filter);
            }
            catch(Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                await UserDialogs.Instance.AlertAsync(new AlertConfig
                {
                    Title ="Filter speichern",
                    Message = ex.Message
                }); 
            }
        });

        public ICommand DeleteCommand => new Command(async () =>
        {
            var result = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig
            {
                Title = "Filter löschen",
                Message = "Diesen Filter wirklich löschen?",
                OkText = "Ja",
                CancelText = "Nein"
            });
            if (result)
            {
                await FilterService.DeleteAsync(Filter);
                await Navigation.PopAsync();
                FilterDeleted?.Invoke(this, Filter);
            }
        });

        public ICommand AddEntryCommand => new Command(async () =>
        {
            var selectionView = Resolver.Resolve<FilterEntrySelectionPage>();
            var viewModel = selectionView.BindingContext as FilterEntrySelectionViewModel;

            viewModel.Title = "Filter hinzufügen";
            viewModel.Filter = Filter;
            viewModel.LoadFilterEntryTypesCommand.Execute(null);

            viewModel.FilterEntryTypeSelected += (sender, filterEntryType) => 
            {
                FilterEntry filterEntry = AddFilterEntry();
                filterEntry.FilterEntryType = filterEntryType; 
                switch(filterEntryType.Property)
                {
                    case FilterEntryProperty.LastSeen:
                    case FilterEntryProperty.ReleaseDate:
                        filterEntry.ValueFrom = filterEntry.ValueTo = DateTime.Today.ToString("d");
                        break;
                    case FilterEntryProperty.Rating:
                        filterEntry.ValueFrom = "1";
                        break;
                    default:
                        filterEntry.ValueFrom = filterEntry.ValueTo = string.Empty;
                        break;
                }

                var grouping = FilterEntries.Where(g => g.Key == filterEntry.FilterEntryType.Name).FirstOrDefault();
                if (grouping == null)
                {
                    var tmpList = new List<FilterDetailItemViewModel>
                    {
                        CreateFilterDetailItemViewModel(filterEntry)
                    };
                    grouping = new Grouping<string, FilterDetailItemViewModel>(filterEntry.FilterEntryType.Name, tmpList);
                    FilterEntries.Insert(0, grouping);
                }
                else grouping.Add(CreateFilterDetailItemViewModel(filterEntry));
            };

            await Navigation.PushAsync(selectionView);
        });

        public ICommand RemoveEntryCommand => new Command(() => RemoveFilterEntries());

        private FilterEntry AddFilterEntry()
        {
            FilterEntry filterEntry = FilterEntry.CreateNew<FilterEntry>();
            filterEntry.Filter = Filter;
            filterEntry.FilterEntryType = new FilterEntryType();
            Filter.FilterEntries.Add(filterEntry);
            return filterEntry;
        }

        private void RemoveFilterEntries()        
        {
            foreach (var groupedItems in FilterEntries.Select(g => g.Items))
            {
                var selectedEntries = groupedItems.Where(m => m.IsSelected == true).ToList();
                for (int i=selectedEntries.Count-1;i>=0;i--)
                    RemoveFilterEntry(selectedEntries[i]);
            }
        }

        private void RemoveFilterEntry(FilterDetailItemViewModel selectedViewModel)
        {
            if (selectedViewModel == null)
                return;

            // Ein neu angelegtes, noch nicht in der DB gespeichertes Objekt muss nicht gelöscht werden
            if (!selectedViewModel.FilterEntry.IsNew)
                selectedViewModel.FilterEntry.IsDeleted = true;
            selectedViewModel.FilterEntry.IsNew = selectedViewModel.FilterEntry.IsModified = false;

            var grouping = FilterEntries.Where(g => g.Key == selectedViewModel.FilterEntry.FilterEntryType.Name).FirstOrDefault();
            if (grouping != null)
                grouping.Remove(CreateFilterDetailItemViewModel(selectedViewModel.FilterEntry));
        }

        private void LoadFilterEntries()
        {
            FilterEntries.Clear();

            IEnumerable<FilterDetailItemViewModel> filterEntries = new List<FilterDetailItemViewModel>(Filter.FilterEntries.Select(f => CreateFilterDetailItemViewModel(f)));

            var sorted = from filterEntry in filterEntries
                         orderby filterEntry.FilterEntry.FilterEntryType.Name
                         group filterEntry by filterEntry.FilterEntry.FilterEntryType.Name into entryGroup
                         select new Grouping<string, FilterDetailItemViewModel>(entryGroup.Key, entryGroup);
            _filterEntries.ReplaceRange(sorted);
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
