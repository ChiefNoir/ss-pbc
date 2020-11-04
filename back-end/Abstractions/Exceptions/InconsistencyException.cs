using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Abstractions.Exceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    /// <summary> Represents the potential inconsistency error in the storage </summary>
    public class InconsistencyException : Exception
    {
        public InconsistencyException() : base()
        {
        }

        public InconsistencyException(string message) : base(message)
        {
        }

        public InconsistencyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InconsistencyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
