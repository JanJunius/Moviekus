using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Moviekus.Models
{
    [DebuggerDisplay("Title = {Title}")]
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

        public override bool Equals(object obj)
        {
            return obj is Movie movie &&
                   base.Equals(obj) &&
                   Title == movie.Title &&
                   EqualityComparer<Source>.Default.Equals(Source, movie.Source) &&
                   EqualityComparer<ICollection<MovieGenre>>.Default.Equals(MovieGenres, movie.MovieGenres);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Title, Source, MovieGenres);
        }
    }
}