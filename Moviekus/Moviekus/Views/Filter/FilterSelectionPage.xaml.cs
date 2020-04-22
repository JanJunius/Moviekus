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
    public partial class FilterSelectionPage : ContentPage
    {
        private FilterSelectionViewModel ViewModel;

        public FilterSelectionPage(FilterSelectionViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = ViewModel = viewModel;
            viewModel.Navigation = Navigation;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (ViewModel.Filter.Count == 0)
                ViewModel.LoadFilterCommand.Execute(null);
        }

    }
}