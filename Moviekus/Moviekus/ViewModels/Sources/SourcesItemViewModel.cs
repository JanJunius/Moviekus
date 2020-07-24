using Moviekus.Models;
using System.Collections.Generic;

namespace Moviekus.ViewModels.Sources
{
    public class SourcesItemViewModel : BaseViewModel
    {
        public Source Source { get; set; }

        public SourcesItemViewModel(Source source)
        {
            Source = source;
        }

        public override bool Equals(object obj)
        {
            return obj is SourcesItemViewModel model &&
                   EqualityComparer<Source>.Default.Equals(Source, model.Source);
        }

        public override int GetHashCode()
        {
            var hashCode = -811082444;
            hashCode = hashCode * -1521134295 + EqualityComparer<Source>.Default.GetHashCode(Source);
            return hashCode;
        }
    }
}
