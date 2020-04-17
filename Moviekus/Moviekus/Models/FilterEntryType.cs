using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Text;

namespace Moviekus.Models
{
    [DebuggerDisplay("Name = {Name}")]
    public class FilterEntryType : BaseModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public FilterEntryProperty Property { get; set; }

        public override bool Equals(object obj)
        {
            return obj is FilterEntryType type &&
                   base.Equals(obj) &&
                   Name == type.Name &&
                   Property == type.Property;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Name, Property);
        }
    }
}
