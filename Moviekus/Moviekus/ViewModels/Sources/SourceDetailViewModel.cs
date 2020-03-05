using Moviekus.Models;
using Moviekus.Services;
using Moviekus.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Moviekus.ViewModels.Sources
{
    public class SourceDetailViewModel : BaseViewModel
    {
        private ISourceService<Source> SourceService;

        public Source Source { get; set; }

        public SourceDetailViewModel(SourceService sourceService)
        {
            SourceService = sourceService;
        }

        public ICommand SaveCommand => new Command(async () =>
        {
            await SourceService.UpdateSourceAsync(Source);
            await Navigation.PopAsync();
        });
    }
}
