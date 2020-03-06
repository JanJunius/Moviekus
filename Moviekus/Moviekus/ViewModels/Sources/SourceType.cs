using System;
using System.Collections.Generic;
using System.Text;

namespace Moviekus.ViewModels.Sources
{
    public class SourceType
    {
        public string Name { get; set; }

        public string ImageUri { get; set; }

        public static IList<SourceType> AvailableSourceTypes
        {
            get
            {
                return new List<SourceType>()
                {
                    new SourceType() { Name = "Lokal", ImageUri = "local.png" },
                    new SourceType() { Name = "Netflix", ImageUri = "netflix.png" },
                    new SourceType() { Name = "Amazon Prime", ImageUri = "prime.png" }
                };
            }
        }


    }
}
