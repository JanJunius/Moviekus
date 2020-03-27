using Moviekus.ViewModels.Genres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Moviekus.Views.Genres
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GenresPage : ContentPage
    {
        private GenresViewModel viewModel;

        public GenresPage(GenresViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
            viewModel.Navigation = Navigation;

            // Zurücksetzen der Selektion eines Genre, damit man es direkt nochmal selektieren kann
            // (andernfalls müsste man erst ein anderes Genre wählen und dann wieder zurück)
            GenresListView.ItemSelected += (sender, args) => GenresListView.SelectedItem = null;

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Genres.Count == 0)
                viewModel.LoadGenresCommand.Execute(null);
        }

    }
}