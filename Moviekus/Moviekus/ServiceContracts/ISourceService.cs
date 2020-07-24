using Moviekus.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Moviekus.ServiceContracts
{
    public interface ISourceService : IBaseService<Source>
    {
        Source CreateSource();
    }
}
