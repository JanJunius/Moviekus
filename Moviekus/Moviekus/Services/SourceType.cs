using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moviekus.Services
{
    public class SourceType
    {
        public string Name { get; set; }

        public string ImageUri { get; set; }

        // Gibt an, ob dieser Typ bei neuen Filmen voreingestellt sein soll
        public bool IsDefault { get; set; }

        public static IList<SourceType> AvailableSourceTypes
        {
            get
            {
                return new List<SourceType>()
                {
                    new SourceType() { Name = "Lokal", ImageUri = "local.png", IsDefault = true },
                    new SourceType() { Name = "Netflix", ImageUri = "netflix.png" },
                    new SourceType() { Name = "Amazon Prime", ImageUri = "prime.png" }
                };
            }
        }

        public static SourceType DefaultSourceType
        {
            get { return AvailableSourceTypes.Where(st => st.IsDefault == true).FirstOrDefault(); }
        }
    }
}
