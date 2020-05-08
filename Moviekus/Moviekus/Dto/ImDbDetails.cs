using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Moviekus.Dto
{
    [DebuggerDisplay("Title = {title}")]
    public class ImDbDetails
    {
        public string title { get; set; }

        public int runtimeMins { get; set; }

        public string plot { get; set; }

        public string genres { get; set; }

        public DateTime releaseDate { get; set; }

        public ImDbTrailer trailer { get; set; }
    }
}
