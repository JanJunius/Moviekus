﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Moviekus.Dto.MovieDb
{
    public class MovieDbDetails
    {
        public string title { get; set; }

        public string poster_path { get; set; }

        public int runtime { get; set; }

        public bool video { get; set; }

        public string homepage { get; set; }
    }
}