using Moviekus.ViewModels.Genres;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Moviekus.Views.Genres
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GenreDetailPage : ContentPage
    {
        private GenreDetailViewModel ViewModel;

        public GenreDetailPage(GenreDetailViewModel viewModel)
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