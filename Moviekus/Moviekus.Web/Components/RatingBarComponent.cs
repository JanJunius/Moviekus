using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Moviekus.Web.Components
{
    public class RatingBarComponent : ComponentBase
    {
        [Parameter] public int Rating { get; set; }
    }
}
