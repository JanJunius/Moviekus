using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Text;

namespace Moviekus.Models
{
    [DebuggerDisplay("Property = {FilterEntryType.Property}")]
    public class FilterEntry : BaseModel
    {
        public FilterEntry()
        {
            // ACHTUNG: DFer FilterEntryType darf hier nicht angelegt werden, sonst wird das Objekt beim Laden nicht korrekt gefüllt!
            //FilterEntryType = new FilterEntryType();
            Operator = FilterEntryOperator.Equal;
        }

        [ForeignKey("FilterId")]
        [Required]
        public Filter Filter { get; set; }


        [ForeignKey("FilterEntryTypeId")]
        [Required]
        public FilterEntryType FilterEntryType { get; set; }

        [Required]
        public string ValueFrom { get; set; }

        public string ValueTo { get; set; }

        public FilterEntryOperator Operator { get; set; }

        public override bool Equals(object obj)
        {
            return obj is FilterEntry entry &&
                   base.Equals(obj) &&
                   EqualityComparer<Filter>.Default.Equals(Filter, entry.Filter) &&
                   EqualityComparer<FilterEntryType>.Default.Equals(FilterEntryType, entry.FilterEntryType) &&
                   ValueFrom == entry.ValueFrom &&
                   ValueTo == entry.ValueTo &&
                   Operator == entry.Operator &&
                   IsDeleted == entry.IsDeleted;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Filter, FilterEntryType, ValueFrom, ValueTo, Operator, IsDeleted);
        }
    }
}
