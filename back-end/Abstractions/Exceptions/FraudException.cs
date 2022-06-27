using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Abstractions.Exceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    /// <summary>  Represents the potential fraud in the system </summary>
    public class FraudException : Exception
    {
        public FraudException() : base()
        {
        }

        public FraudException(string message) : base(message)
        {
        }

        public FraudException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FraudException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}