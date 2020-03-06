using Moviekus.ViewModels;
using Moviekus.ViewModels.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Moviekus.Views.Sources
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SourcesPage : ContentPage
    {
        SourcesViewModel viewModel;

        public SourcesPage(SourcesViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
            viewModel.Navigation = Navigation;

            // Zurücksetzen der Selektion einer Quelle, damit man sie direkt nochmal selektieren kann
            // (andernfalls müsste man erst eine andere Quelle wählen und dann wieder zurück)
            SourcesListView.ItemSelected += (sender, args) => SourcesListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Sources.Count == 0)
                viewModel.LoadSourcesCommand.Execute(null);
        }

    }
}