using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Moviekus.Dto
{
    [DebuggerDisplay("Title = {Title}")]
    public class MovieDto
    {
        public MovieDto()
        {
            Genres = new List<string>();
        }

        public string MovieDbId { get; set; }
        
        public string ImDbId { get; set; }

        public string Title { get; set; }

        public string Overview { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string ReleaseYear
        {
            get 
            { 
                if (ReleaseDate != null && ReleaseDate != DateTime.MinValue) 
                    return ReleaseDate.Year.ToString();
                return string.Empty;
            }
        }

        public IList<string> Genres { get; set; }

        public int Runtime { get; set; }

        public byte[] Cover { get; set; }


    }
}
