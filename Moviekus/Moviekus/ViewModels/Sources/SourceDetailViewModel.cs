using Acr.UserDialogs;
using Moviekus.Models;
using Moviekus.Services;
using Moviekus.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Moviekus.ViewModels.Sources
{
    public class SourceDetailViewModel : BaseViewModel
    {
        private IService<Source> SourceService;

        public Source Source { get; set; }

        public SourceDetailViewModel(SourceService sourceService)
        {
            SourceService = sourceService;
        }

        public ICommand SaveCommand => new Command(async () =>
        {
            await SourceService.UpdateAsync(Source);
            await Navigation.PopAsync();
        });

        public ICommand DeleteCommand => new Command(async () =>
        {
            var result = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig
            { 
                Message = "Diese Quelle wirklich löschen?",
                OkText = "Ja",
                CancelText = "Nein"
            });
            if (result)
            {
                await SourceService.DeleteAsync(Source);
                await Navigation.PopAsync();
            }
        });

        public IList<SourceType> SourceTypes
        {
            get
            {
                return SourceType.AvailableSourceTypes;
            }
        }

        private SourceType _selectedSourceType;

        public SourceType SelectedSourceType
        {
            get 
            {
                /*
                if (Source == null)
                    return null;
                return SourceType.AvailableSourceTypes.FirstOrDefault(s => s.Name == Source.SourceTypeName);
                */
                return _selectedSourceType;
            }
            set
            {
                if (Source != null && value != null)
                {
                    SetProperty(ref _selectedSourceType, value);
                    Source.SourceTypeName = value.Name;
                }
                    
            }
        }

    }
}
