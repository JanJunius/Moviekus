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
    public partial class SourceDetailPage : ContentPage
    {
        private SourceDetailViewModel ViewModel;

        public SourceDetailPage(SourceDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = ViewModel = viewModel;
            viewModel.Navigation = Navigation;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ViewModel.OnViewDisappearing();
        }
    }
}