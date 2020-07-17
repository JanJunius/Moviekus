using System;
using System.Collections.Generic;
using System.Text;

namespace Moviekus.Dto.MovieDb
{
    public class MovieDbResponse
    {
        public string page { get; set; }

        public string total_results { get; set; }

        public List<MovieDbResult> results { get; set; }
    }
}
