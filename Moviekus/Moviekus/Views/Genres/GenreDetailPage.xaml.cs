using Moviekus.ViewModels.Genres;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Moviekus.Views.Genres
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GenreDetailPage : ContentPage
    {
        private GenreDetailViewModel viewModel;

        public GenreDetailPage(GenreDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
            viewModel.Navigation = Navigation;
        }
    }
}