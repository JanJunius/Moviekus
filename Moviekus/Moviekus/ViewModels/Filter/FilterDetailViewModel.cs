using Acr.UserDialogs;
using Moviekus.Dto;
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
                BuildFilterDto();
            }
        }

        public FilterDetailViewModel(FilterService filterService)
        {
            FilterService = filterService;
            FilterEntries = new ObservableCollection<FilterDetailItemViewModel>();
        }

        public ObservableCollection<FilterDetailItemViewModel> FilterEntries { get; set; }

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

        private void BuildFilterDto()
        {
            FilterEntries.Clear();
            foreach(var filterEntry in Filter.FilterEntries)
            {
                var filterDetailItemViewModel = new FilterDetailItemViewModel()
                {
                    FilterEntry = filterEntry
                };
                FilterEntries.Add(filterDetailItemViewModel);
            }
        }

    }
}
