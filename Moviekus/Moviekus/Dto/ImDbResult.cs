using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Moviekus.Dto
{
    [DebuggerDisplay("Title = {title}")]
    public class ImDbResult
    {
        public string id { get; set; }

        public string title { get; set; }

        public string image { get; set; }

    }
}
