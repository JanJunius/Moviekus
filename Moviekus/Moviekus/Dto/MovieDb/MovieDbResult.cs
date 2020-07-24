using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Moviekus.Dto.MovieDb
{
    [DebuggerDisplay("Title = {title}")]
    public class MovieDbResult
    {
        public string id { get; set; }

        public string title { get; set; }
        
        public string original_title { get; set; }
        
        public string poster_path { get; set; }

        public string original_language { get; set; }

        public string overview { get; set; }

        public DateTime release_date { get; set; }

        public List<string> genre_ids { get; set; }
    }
}
