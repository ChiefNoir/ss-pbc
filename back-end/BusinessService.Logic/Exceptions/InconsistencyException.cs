using System;

namespace BusinessService.Logic.Exceptions
{
    /// <summary>Probability of inconsistency in database, transaction must me prevented or rollbacked </summary>
    public class InconsistencyException : Exception
    {
        /// <summary> Initializes a new instance of the <seealso cref="InconsistencyException"/> class</summary>
        public InconsistencyException() { }

        /// <summary> Initializes a new instance of the <seealso cref="InconsistencyException"/> class with a specified error message. </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public InconsistencyException(string message) : base(message) { }

        /// <summary> Initializes a new instance of the <seealso cref="InconsistencyException"/>  class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public InconsistencyException(string message, Exception innerException) : base(message, innerException) { }
    }
}
