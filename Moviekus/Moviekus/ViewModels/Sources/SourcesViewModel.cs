using Moviekus.Models;
using Moviekus.Services;
using Moviekus.Views.Sources;
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
        private ISourceService<Source> SourceService;

        public ObservableCollection<Source> Sources { get; set; }
        
        public ICommand LoadSourcesCommand => new Command(async () =>
        {
            await LoadSources();
        });

        public ICommand AddSourceCommand => new Command(async () =>
        {
            var sourceDetailView = Resolver.Resolve<SourceDetailPage>();
            var viewModel = sourceDetailView.BindingContext as SourceDetailViewModel;
            viewModel.Source = Source.CreateNewModel<Source>(); // Setzt IsNew=true
            viewModel.Title = "Neue Quelle erfassen";           
            
            await Navigation.PushAsync(sourceDetailView);
        });

        public SourcesViewModel(SourceService sourceService)
        {
            Title = "Quellen";
            Sources = new ObservableCollection<Source>();
            SourceService = sourceService;

            // Wiederspiegeln der Datenbankänderungen in der Liste
            sourceService.OnModelInserted += (sender, source) => Sources.Add(source);
            sourceService.OnModelUpdated += async (sender, source) => await LoadSources();
            sourceService.OnModelDeleted +=  (sender, source) => Sources.Remove(source);
        }

        // Dient lediglich dazu, auf die Auswahl einer Quelle zu reagieren
        // Angesteuert über Binding in der Page
        public Source SelectedItem
        {
            get { return null; }
            set
            {
                if (value != null)
                {
                    Device.BeginInvokeOnMainThread(async () => await OpenDetalPage(value));
                    RaisePropertyChanged(nameof(SelectedItem));
                }
            }
        }

        private async Task OpenDetalPage(Source source)
        {
            var detailView = Resolver.Resolve<SourceDetailPage>();
            var viewModel = detailView.BindingContext as SourceDetailViewModel;
            viewModel.Source = source;

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
                var sources = await SourceService.GetSourcesAsync();
                foreach (var source in sources)
                {
                    Sources.Add(source);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
