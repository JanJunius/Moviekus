using System;
using System.Collections.Generic;
using System.Text;

namespace Moviekus.Dto
{
    public class MovieDbVideos
    {
        public string id { get; set; }

        public List<MovieDbVideo> results { get; set; }

    }
}
