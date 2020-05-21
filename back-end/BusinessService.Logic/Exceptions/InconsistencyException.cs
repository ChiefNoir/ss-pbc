using System;

namespace BusinessService.Logic.Exceptions
{
    public class InconsistencyException : Exception
    {
        public InconsistencyException(string message) : base(message) { }

        public InconsistencyException() { }

        public InconsistencyException(string message, Exception innerException) : base(message, innerException) { }
    }
}
