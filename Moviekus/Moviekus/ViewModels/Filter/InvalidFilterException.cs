using System;

namespace Moviekus.ViewModels.Filter
{
    public class InvalidFilterException : ApplicationException
    {
        public InvalidFilterException(string msg) : base(msg)
        {
        }
    }
}
