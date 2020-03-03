using Moviekus.Models;
using Moviekus.Services;
using Moviekus.Views.Sources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Moviekus.ViewModels
{
    public class SourcesViewModel : BaseViewModel
    {
        public ISourceService<Source> DataStore => DependencyService.Get<ISourceService<Source>>();

        public ObservableCollection<Source> Sources { get; set; }
        public Command LoadSourcesCommand { get; set; }

        public SourcesViewModel()
        {
            Title = "Quellen";
            Sources = new ObservableCollection<Source>();
            LoadSourcesCommand = new Command(async () => await ExecuteLoadSourcesCommand());

            MessagingCenter.Subscribe<SourceDetailPage, Source>(this, "Neu", async (obj, source) =>
            {
                var newSource = source as Source;
                Sources.Add(newSource);
                await DataStore.AddSourceAsync(newSource);
            });
        }

        async Task ExecuteLoadSourcesCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Sources.Clear();
                var sources = await DataStore.GetSourcesAsync(true);
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
