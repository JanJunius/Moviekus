using Moviekus.ViewModels.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Moviekus.Views.Filter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilterDetailPage : ContentPage
    {
        private FilterDetailViewModel ViewModel;

        public FilterDetailPage(FilterDetailViewModel viewModel)
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