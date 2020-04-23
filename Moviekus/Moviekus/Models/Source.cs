using System.Collections.Generic;
using System.Diagnostics;

namespace Moviekus.Models
{
    [DebuggerDisplay("Name = {Name}, SourceType = {SourceTypeName}")]
    public class Source : BaseModel
    {
        public string Name { get; set; }

        public string SourceTypeName { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Source source &&
                   base.Equals(obj) &&
                   Name == source.Name && 
                   SourceTypeName == source.SourceTypeName;
        }

        public virtual ICollection<Movie> Movies { get; set; }

        public override int GetHashCode()
        {
            var hashCode = 890389916;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SourceTypeName);
            return hashCode;
        }
    }
}
