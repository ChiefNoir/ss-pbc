using System;

namespace Abstractions.Exceptions
{
    /// <summary> Represents the potential inconsistency error in the storage </summary>
    public class InconsistencyException : Exception
    {
        public InconsistencyException(string message, Exception innerException = null):base(message, innerException)
        {
        }
    }
}
