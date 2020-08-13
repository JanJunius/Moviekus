using Moviekus.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Moviekus.Dto.MovieDb
{
    public class MovieDbDetails
    {
        public string id { get; set; }

        public string title { get; set; }

        public string poster_path { get; set; }

        public int runtime { get; set; }

        public bool video { get; set; }

        public string homepage { get; set; }

        public string overview { get; set; }

        public DateTime release_date { get; set; }

        public IList<MovieDbGenre> Genres { get; set; }
    }
}
