using Moviekus.Models;
using Moviekus.Services;
using Moviekus.Views.Sources;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Moviekus.ViewModels.Sources
{
    public class SourcesViewModel : BaseViewModel
    {
        private IService<Source> SourceService;

        public ObservableCollection<SourcesItemViewModel> Sources { get; set; }
        
        public ICommand LoadSourcesCommand => new Command(async () =>
        {
            await LoadSources();
        });

        public ICommand AddSourceCommand => new Command(async () =>
        {
            var sourceDetailView = Resolver.Resolve<SourceDetailPage>();
            var viewModel = sourceDetailView.BindingContext as SourceDetailViewModel;
            viewModel.Source = Source.CreateNew<Source>();
            viewModel.Title = "Neue Quelle";           
            
            await Navigation.PushAsync(sourceDetailView);
        });

        public SourcesViewModel(SourceService sourceService)
        {
            Title = "Quellen";
            Sources = new ObservableCollection<SourcesItemViewModel>();
            SourceService = sourceService;

            // Wiederspiegeln der Datenbankänderungen in der Liste
            sourceService.OnModelInserted += (sender, source) => Sources.Add(CreateSourcesItemViewModel(source));
            sourceService.OnModelUpdated += async (sender, source) => await LoadSources();
            sourceService.OnModelDeleted +=  (sender, source) => Sources.Remove(CreateSourcesItemViewModel(source));
        }

        // Dient lediglich dazu, auf die Auswahl einer Quelle zu reagieren
        // Angesteuert über Binding in der Page
        public SourcesItemViewModel SelectedItem
        {
            get { return null; }
            set
            {
                if (value != null)
                {
                    Device.BeginInvokeOnMainThread(async () => await OpenDetailPage(value));
                    RaisePropertyChanged(nameof(SelectedItem));
                }
            }
        }

        private async Task OpenDetailPage(SourcesItemViewModel siViewModel)
        {
            var detailView = Resolver.Resolve<SourceDetailPage>();
            var viewModel = detailView.BindingContext as SourceDetailViewModel;
            viewModel.Source = siViewModel.Source;
            viewModel.Title = "Quelle bearbeiten";

            await Navigation.PushAsync(detailView);
        }

        private async Task LoadSources()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Sources.Clear();
                var sources = await SourceService.GetAsync();

                var itemViewModels = sources.Select(m => CreateSourcesItemViewModel(m)).OrderBy(s => s.Source.Name);
                Sources = new ObservableCollection<SourcesItemViewModel>(itemViewModels);
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private SourcesItemViewModel CreateSourcesItemViewModel(Source source)
        {
            var sourcesItemViewModel = new SourcesItemViewModel(source);
            return sourcesItemViewModel;
        }

    }
}
