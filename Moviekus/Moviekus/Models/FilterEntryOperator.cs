using System;
using System.Collections.Generic;
using System.Text;

namespace Moviekus.Models
{
    public enum FilterEntryOperator
    {
        Equal = 0,
        NotEqual = 1,
        Greater = 2,
        Lesser = 3,
        Contains = 4,
        Between = 5
    }
}
