using System;
using System.Diagnostics.CodeAnalysis;

namespace Abstractions.Exceptions
{
    [ExcludeFromCodeCoverage]
    /// <summary> Represents the potential inconsistency error in the storage </summary>
    public class InconsistencyException : Exception
    {
        public InconsistencyException(string message, Exception innerException = null):base(message, innerException)
        {
        }
    }
}
