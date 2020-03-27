using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moviekus.Models
{
    public class Movie : BaseModel
    {
        [Required]
        public string Title { get; set; }
        
        public Source Source { get; set; }

        public virtual ICollection<MovieGenre> MovieGenres { get; set; }

        public Movie()
        {
            Source = CreateNew<Source>();
            MovieGenres = new List<MovieGenre>();
        }

    }
}