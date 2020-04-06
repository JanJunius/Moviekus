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
    public partial class FilterPage : ContentPage
    {
        private FilterViewModel ViewModel;

        public FilterPage(FilterViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = ViewModel = viewModel;
            viewModel.Navigation = Navigation;

            // Zurücksetzen der Selektion eines Films, damit man ihn direkt nochmal selektieren kann
            // (andernfalls müsste man erst einen anderen Film wählen und dann wieder zurück)
            FilterList.ItemSelected += (sender, args) => FilterList.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ViewModel.LoadFilterCommand.Execute(null);
        }

    }
}