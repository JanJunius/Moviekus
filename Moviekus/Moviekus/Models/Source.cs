﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Moviekus.Models
{
    [DebuggerDisplay("Name = {Name}, SourceType = {SourceTypeName}")]
    public class Source : BaseModel
    {
        [MinLength(1, ErrorMessage = "Der Name darf nicht leer sein.")]
        [Required(ErrorMessage = "Bitte einen Namen angeben.")]
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Source source &&
                   base.Equals(obj) &&
                   Name == source.Name;
        }

        public virtual ICollection<Movie> Movies { get; set; }

        public override int GetHashCode()
        {
            var hashCode = 890389916;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            return hashCode;
        }
    }
}
