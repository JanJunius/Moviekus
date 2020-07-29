using System;

namespace Moviekus.FilterBuilder
{
    public class InvalidFilterException : ApplicationException
    {
        public InvalidFilterException(string msg) : base(msg)
        {
        }
    }
}
