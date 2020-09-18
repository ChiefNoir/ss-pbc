using System;
using System.Collections.Generic;
using System.Text;

namespace Abstractions.Exceptions
{
    /// <summary> Represents the security error </summary>
    public class SecurityException : Exception
    {
        public SecurityException(string message, Exception innerException = null) : base(message, innerException)
        {
        }
    }
}
