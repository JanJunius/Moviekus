using Moviekus.ViewModels.Movies;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Moviekus.Views.Movies
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MovieSelectionPage : ContentPage
    {
        public MovieSelectionPage(MovieSelectionViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = viewModel;
            viewModel.Navigation = Navigation;
        }
    }
}