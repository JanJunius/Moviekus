using Moviekus.ViewModels.Filter;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Moviekus.Views.Filter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilterEntrySelectionPage : ContentPage
    {
        private FilterEntrySelectionViewModel ViewModel;

        public FilterEntrySelectionPage(FilterEntrySelectionViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = ViewModel = viewModel;
            viewModel.Navigation = Navigation;
        }
    }
}