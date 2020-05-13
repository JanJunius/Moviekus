using Moviekus.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Moviekus.Dto
{
    [DebuggerDisplay("Operator = {Operator}")]
    public class FilterEntryOperatorListItem
    {
        public FilterEntryOperator Operator { get; set; }

        public string OperatorName { get; set; }
    }
}
