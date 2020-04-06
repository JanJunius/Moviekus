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
            FilterEntries = new ObservableCollection<FilterDto>();
        }

        public ObservableCollection<FilterDto> FilterEntries { get; set; }

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
            foreach(FilterEntry filterEntry in Filter.FilterEntries)
            {
                FilterDto filterDto = new FilterDto()
                {
                    FilterEntryId = filterEntry.Id,
                    FilterEntryText = GetText(filterEntry)
                };
                FilterEntries.Add(filterDto);
            }
        }

        private string GetText(FilterEntry filterEntry)
        {
            string text = Enum.GetName(typeof(FilterEntryProperty), filterEntry.FilterEntryProperty);

            if (filterEntry.FilterEntryProperty == FilterEntryProperty.LastSeen 
                || filterEntry.FilterEntryProperty == FilterEntryProperty.ReleaseDate)
            {
                text += GetDateText(filterEntry);
            } else if (filterEntry.FilterEntryProperty == FilterEntryProperty.Rating)
            {
                text += GetRatingText(filterEntry);
            } else if (filterEntry.FilterEntryProperty == FilterEntryProperty.Runtime)
            {
                text += GetRuntimeText(filterEntry);
            }
            else
            {
                text += " ist gleich ";
                text += filterEntry.ValueFrom;
            }


            return text;
        }

        private string GetRatingText(FilterEntry filterEntry)
        {
            if (string.IsNullOrEmpty(filterEntry.ValueTo))
            {
                int rating = int.Parse(filterEntry.ValueFrom);
                if (rating < 1)
                    return " nicht bewertet";
                return $" hat {rating} Sterne";
            }
            else
            {
                int ratingFrom = int.Parse(filterEntry.ValueFrom);
                int ratingTo = int.Parse(filterEntry.ValueTo);
                return $" liegt zwischen {ratingFrom} und {ratingTo} Sternen";

            }
        }

        private string GetRuntimeText(FilterEntry filterEntry)
        {
            if (string.IsNullOrEmpty(filterEntry.ValueTo))
            {
                int runtime = int.Parse(filterEntry.ValueFrom);
                return $" beträgt {runtime} Minuten";
            }
            else
            {
                int runtimeFrom = int.Parse(filterEntry.ValueFrom);
                int runtimeTo = int.Parse(filterEntry.ValueTo);
                return $" liegt zwischen {runtimeFrom} und {runtimeTo} Minuten";

            }
        }

        private string GetDateText(FilterEntry filterEntry)
        {
            if (string.IsNullOrEmpty(filterEntry.ValueTo))
            {
                DateTime dt = DateTime.Parse(filterEntry.ValueFrom);
                if (dt == DateTime.MinValue)
                    return " ist leer";
                return $" ist am {dt.ToString("d", MoviekusDefines.MoviekusCultureInfo)}";
            }
            else
            {
                DateTime dtFrom = DateTime.Parse(filterEntry.ValueFrom);
                DateTime dtTo = DateTime.Parse(filterEntry.ValueTo);
                return $" liegt zwischen dem {dtFrom.ToString("d", MoviekusDefines.MoviekusCultureInfo)} und dem {dtTo.ToString("d", MoviekusDefines.MoviekusCultureInfo)}";

            }
        }
    }
}
