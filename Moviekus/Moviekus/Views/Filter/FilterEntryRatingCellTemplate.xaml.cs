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
    public partial class FilterEntryRatingCellTemplate : ViewCell
    {
        public FilterEntryRatingCellTemplate()
        {
            InitializeComponent();
        }
    }
}