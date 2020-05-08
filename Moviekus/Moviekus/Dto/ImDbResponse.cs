using System;
using System.Collections.Generic;
using System.Text;

namespace Moviekus.Dto
{
    public class ImDbResponse
    {
        public string searchType { get; set; }

        public string expression { get; set; }

        public List<ImDbResult> results { get; set; }
    }
}
