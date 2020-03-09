using Moviekus.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Moviekus.ViewModels.Sources
{
    public class SourcesItemViewModel : BaseViewModel
    {
        public Source Source { get; set; }

        public string ImageUri
        {
            get
            {
                if (Source == null)
                    return null;
                SourceType sourceType = SourceType.AvailableSourceTypes.FirstOrDefault(s => s.Name == Source.SourceTypeName);
                return sourceType?.ImageUri;
            }
        }

        public SourcesItemViewModel(Source source)
        {
            Source = source;
        }

        public override bool Equals(object obj)
        {
            return obj is SourcesItemViewModel model &&
                   EqualityComparer<Source>.Default.Equals(Source, model.Source) &&
                   ImageUri == model.ImageUri;
        }

        public override int GetHashCode()
        {
            var hashCode = -811082444;
            hashCode = hashCode * -1521134295 + EqualityComparer<Source>.Default.GetHashCode(Source);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ImageUri);
            return hashCode;
        }
    }
}
