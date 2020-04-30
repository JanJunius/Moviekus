using Moviekus.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Moviekus.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LogViewerPage : ContentPage
    {
        private LogViewerViewModel ViewModel;

        public LogViewerPage(LogViewerViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = ViewModel = viewModel;
            viewModel.Navigation = Navigation;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ViewModel.LoadLogCommand.Execute(null);
        }

    }
}