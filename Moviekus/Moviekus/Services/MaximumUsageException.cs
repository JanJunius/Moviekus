using System;
using System.Collections.Generic;
using System.Text;

namespace Moviekus.Services
{
    public class MaximumUsageException : Exception
    {
        public MaximumUsageException(string msg) : base(msg)
        {
        }
    }
}
