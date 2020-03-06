using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Moviekus.Models
{
    public class Source : BaseModel
    {
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Source source &&
                   base.Equals(obj) &&
                   Name == source.Name;
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
