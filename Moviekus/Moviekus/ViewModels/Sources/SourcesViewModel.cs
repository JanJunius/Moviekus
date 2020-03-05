using Moviekus.Models;
using Moviekus.Services;
using Moviekus.Views.Sources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        
        //public Command LoadSourcesCommand { get; set; }

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
            
            //LoadSourcesCommand = new Command(async () => await ExecuteLoadSourcesCommand());

            /*
            MessagingCenter.Subscribe<SourceDetailPage, Source>(this, "Neu", async (obj, source) =>
            {
                var newSource = source as Source;
                Sources.Add(newSource);
                await DataStore.AddSourceAsync(newSource);
            });
            */
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
