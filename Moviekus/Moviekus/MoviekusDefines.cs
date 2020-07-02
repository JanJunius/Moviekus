using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Moviekus
{
    public class MoviekusDefines
    {
        public static readonly string DbFileName = "Moviekus.db3";

        public static CultureInfo MoviekusCultureInfo = new CultureInfo("de-DE");

        public static readonly DateTime MinDate = new DateTime(1900, 1, 1);

        public static readonly string SourceLokal = "4f72aa31-38b3-48b6-988e-3f96df94c44f";
        public static readonly string SourceDisk = "b085069d-7e7d-4725-967c-c5dc8b21e70b";
        public static readonly string SourceNetflix = "79aab413-5c1b-4446-9288-72f0f476b219";
        public static readonly string SourcePrime = "3fe0f150-c349-4462-99fd-0e61e6e513a7";
    }
}
