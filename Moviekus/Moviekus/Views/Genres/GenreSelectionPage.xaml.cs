
using Moviekus.ViewModels.Genres;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Moviekus.Views.Genres
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GenreSelectionPage : ContentPage
    {
        private GenreSelectionViewModel _viewModel;

        public GenreSelectionPage(GenreSelectionViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = _viewModel = viewModel;
            _viewModel.Navigation = Navigation;
        }
    }
}