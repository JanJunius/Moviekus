using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Moviekus.Dto.MovieDb
{
    [DebuggerDisplay("Title = {Title}")]
    public class MovieDbMovie
    {
        public MovieDbMovie()
        {
            Genres = new List<string>();
        }

        public string ProviderMovieId { get; set; }
        
        public string ImDbId { get; set; }

        public string Title { get; set; }

        public string Overview { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string ReleaseYear
        {
            get 
            { 
                if (ReleaseDate != null && ReleaseDate != MoviekusDefines.MinDate) 
                    return ReleaseDate.Year.ToString();
                return string.Empty;
            }
        }

        public IList<string> Genres { get; set; }

        public string Genre
        {
            get
            {
                string genre = string.Empty;
                foreach(var g in Genres)
                    genre += g+"; ";
                return genre.Length > 1 ? genre.Substring(0, genre.Length - 2) : genre;
            }
        }

        public int Runtime { get; set; }

        public byte[] Cover { get; set; }

        public string CoverUri { get; set; }

        public string Homepage { get; set; }
        
        public string TrailerUrl { get; set; }


        public override bool Equals(object obj)
        {
            return obj is MovieDbMovie dto &&
                   ProviderMovieId == dto.ProviderMovieId &&
                   ImDbId == dto.ImDbId &&
                   Title == dto.Title &&
                   Overview == dto.Overview &&
                   ReleaseDate == dto.ReleaseDate &&
                   EqualityComparer<IList<string>>.Default.Equals(Genres, dto.Genres) &&
                   Runtime == dto.Runtime &&
                   EqualityComparer<byte[]>.Default.Equals(Cover, dto.Cover) &&
                   CoverUri == dto.CoverUri && 
                   Homepage == dto.Homepage &&
                   TrailerUrl == dto.TrailerUrl;
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(ProviderMovieId);
            hash.Add(ImDbId);
            hash.Add(Title);
            hash.Add(Overview);
            hash.Add(ReleaseDate);
            hash.Add(Genres);
            hash.Add(Runtime);
            hash.Add(Cover);
            hash.Add(CoverUri);
            hash.Add(Homepage);
            hash.Add(TrailerUrl);
            return hash.ToHashCode();
        }
    }
}
