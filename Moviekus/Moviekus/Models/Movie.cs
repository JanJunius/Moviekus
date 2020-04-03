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

        public string Description { get; set; }

        public DateTime ReleaseDate { get; set; }

        public int Runtime { get; set; }

        public int Rating { get; set; }

        public DateTime LastSeen { get; set; }

        public string Notes { get; set; }

        public byte[] Cover { get; set; }

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
                   Description == movie.Description &&
                   ReleaseDate == movie.ReleaseDate &&
                   Runtime == movie.Runtime &&
                   Rating == movie.Rating &&
                   LastSeen == movie.LastSeen &&
                   Notes == movie.Notes &&
                   EqualityComparer<ICollection<MovieGenre>>.Default.Equals(MovieGenres, movie.MovieGenres);
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(base.GetHashCode());
            hash.Add(Title);
            hash.Add(Source);
            hash.Add(Description);
            hash.Add(ReleaseDate);
            hash.Add(Runtime);
            hash.Add(Rating);
            hash.Add(LastSeen);
            hash.Add(Notes);
            hash.Add(MovieGenres);
            return hash.ToHashCode();
        }
    }
}