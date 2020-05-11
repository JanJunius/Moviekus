using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text;

namespace Moviekus.Models
{
    [DebuggerDisplay("Name = {Name}")]
    public class Filter : BaseModel
    {
        [Required]
        public string Name { get; set;  }

        public bool IsDefault { get; set; }

        public ICollection<FilterEntry> FilterEntries { get; set; }

        public Filter()
        {
            FilterEntries = new List<FilterEntry>();
        }

        public override bool Equals(object obj)
        {
            return obj is Filter filter &&
                   base.Equals(obj) &&
                   Name == filter.Name &&
                   IsDefault == filter.IsDefault &&
                   EqualityComparer<ICollection<FilterEntry>>.Default.Equals(FilterEntries, filter.FilterEntries);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Name, IsDefault, FilterEntries);
        }
    }
}
