using Moviekus.ViewModels;
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
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Sources.Count == 0)
                viewModel.LoadSourcesCommand.Execute(null);
        }

    }
}