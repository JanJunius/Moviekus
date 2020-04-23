using Acr.UserDialogs;
using Moviekus.Dto;
using Moviekus.Models;
using Moviekus.Services;
using Moviekus.Views.Filter;
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
            FilterEntries = new ObservableCollection<FilterDetailItemViewModel>();
        }

        public ObservableCollection<FilterDetailItemViewModel> FilterEntries { get; set; }

        public FilterDetailItemViewModel SelectedFilterEntry { get; set; }

        public ICommand SaveCommand => new Command(async () =>
        {
            try
            {
                await FilterService.SaveChangesAsync(Filter);
                await Navigation.PopAsync();
            }
            catch(Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(new AlertConfig
                {
                    Message = ex.Message
                }); 
            }
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
                FilterEntries.Insert(0, CreateFilterDetailItemViewModel(filterEntry));
            };

            await Navigation.PushAsync(selectionView);
        });

        public ICommand RemoveEntryCommand => new Command(async () =>
        {
            await RemoveFilterEntry();
        });

        private FilterEntry AddFilterEntry()
        {
            FilterEntry filterEntry = FilterEntry.CreateNew<FilterEntry>();
            filterEntry.Filter = Filter;
            filterEntry.FilterEntryType = new FilterEntryType();
            Filter.FilterEntries.Add(filterEntry);
            return filterEntry;
        }

        private async Task RemoveFilterEntry()        {
            if (SelectedFilterEntry == null)
            {
                await UserDialogs.Instance.AlertAsync(new AlertConfig
                {
                    Message = "Bitte den zu entfernenden Eintrag auswählen"
                });
                return;
            }
            // Ein neu angelegtes, noch nicht in der DB gespeichertes Objekt muss nicht gelöscht werden
            if (!SelectedFilterEntry.FilterEntry.IsNew)
                SelectedFilterEntry.FilterEntry.IsDeleted = true;
            SelectedFilterEntry.FilterEntry.IsNew = SelectedFilterEntry.FilterEntry.IsModified = false;

            FilterEntries.Remove(CreateFilterDetailItemViewModel(SelectedFilterEntry.FilterEntry));

            bool removed = Filter.FilterEntries.Remove(SelectedFilterEntry.FilterEntry);
        }

        private void LoadFilterEntries()
        {
            FilterEntries.Clear();
            foreach (var filterEntry in Filter.FilterEntries.OrderBy(p => p.FilterEntryType.Name))
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
