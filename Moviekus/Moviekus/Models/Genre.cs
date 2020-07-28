using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text;

namespace Moviekus.Models
{
    [DebuggerDisplay("Name = {Name}")]
    public class Genre : BaseModel
    {
        [MinLength(1, ErrorMessage ="Der Name darf nicht leer sein.")]
        [Required(ErrorMessage = "Bitte einen Namen angeben.")]
        public string Name { get; set; }

        //public virtual ICollection<MovieGenre> MovieGenres { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Genre genre &&
                   base.Equals(obj) &&
                   Name == genre.Name;
        }

        public override int GetHashCode()
        {
            var hashCode = 890389916;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            return hashCode;
        }
    }
}
