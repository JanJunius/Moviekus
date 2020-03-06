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
    }
}
