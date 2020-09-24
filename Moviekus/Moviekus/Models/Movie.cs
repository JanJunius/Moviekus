using Moviekus.Models.Validation;
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
        [Required(ErrorMessage ="Der Titel darf nicht leer sein.")]
        [Display(Name = "Titel")] 
        public string Title { get; set; }

        [ForeignKey("SourceId")]
        [Display(Name = "Verfügbar bei")] 
        public virtual Source Source { get; set; }

        [Display(Name = "Beschreibung")] 
        public string Description { get; set; }

        [Display(Name = "Veröffentlicht")]
        [DataType(DataType.Date)] 
        public DateTime ReleaseDate { get; set; }

        [Display(Name = "Laufzeit")]
        public int Runtime { get; set; }

        [RatingValidation]
        [Display(Name = "Bewertung")]
        public int Rating { get; set; }

        [Display(Name = "Zuletzt gesehen")]
        [DataType(DataType.Date)] 
        public DateTime LastSeen { get; set; }

        [Display(Name = "Bemerkungen")]
        public string Remarks { get; set; }

        public byte[] Cover { get; set; }

        public string Homepage { get; set; }

        public string Trailer { get; set; }

        [DiscNrValidation]
        [Display(Name = "Disk-Nr.")]
        public string DiscNumber { get; set; }

        [Display(Name = "Episode")]
        public string EpisodeNumber { get; set; }

        [Display(Name = "Genres")]
        public virtual ICollection<MovieGenre> MovieGenres { get; set; }

        public Movie()
        {
            //ACHTUNG: Die Source darf hier nicht angelegt werden, sonst wird das Objekt beim Laden nicht korrekt gefüllt!
            //Source = CreateNew<Source>();
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
                   Remarks == movie.Remarks &&
                   EqualityComparer<byte[]>.Default.Equals(Cover, movie.Cover) &&
                   Homepage == movie.Homepage &&
                   Trailer == movie.Trailer &&
                   DiscNumber == movie.DiscNumber &&
                   EpisodeNumber == movie.EpisodeNumber &&
                   EqualityComparer<ICollection<MovieGenre>>.Default.Equals(MovieGenres, movie.MovieGenres);
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(base.GetHashCode());
            hash.Add(Title);
            hash.Add(Source);
            hash.Add(Description);
            hash.Add(ReleaseDate);
            hash.Add(Runtime);
            hash.Add(Rating);
            hash.Add(LastSeen);
            hash.Add(Remarks);
            hash.Add(Cover);
            hash.Add(Homepage);
            hash.Add(Trailer);
            hash.Add(DiscNumber);
            hash.Add(EpisodeNumber);
            hash.Add(MovieGenres);
            return hash.ToHashCode();
        }
    }
}