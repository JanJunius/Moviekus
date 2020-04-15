using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Text;

namespace Moviekus.Models
{
    [DebuggerDisplay("Property = {FilterEntryProperty}")]
    public class FilterEntry : BaseModel
    {
        [ForeignKey("FilterId")]
        [Required]

        public Filter Filter { get; set; }

        [Required]
        public FilterEntryProperty FilterEntryProperty { get; set; }

        [Required]
        public string ValueFrom { get; set; }

        public string ValueTo { get; set; }

        [NotMapped]
        public bool IsDeleted { get; set; }

        public override bool Equals(object obj)
        {
            return obj is FilterEntry entry &&
                   base.Equals(obj) &&
                   EqualityComparer<Filter>.Default.Equals(Filter, entry.Filter) &&
                   FilterEntryProperty == entry.FilterEntryProperty &&
                   ValueFrom == entry.ValueFrom &&
                   ValueTo == entry.ValueTo &&
                   IsDeleted == entry.IsDeleted;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Filter, FilterEntryProperty, ValueFrom, ValueTo, IsDeleted);
        }
    }
}
