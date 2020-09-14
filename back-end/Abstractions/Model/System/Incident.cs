namespace Abstractions.Model.System
{
    /// <summary> Wrapper for exceptional situation</summary>
    public sealed class Incident
    {
        /// <summary> Get situation code <seealso cref="IncidentsCodes"/> </summary>
        public int Code { get; private set; }

        /// <summary> Get situation message </summary>
        public string Message { get; internal set; }

        /// <summary> Get or set additional details</summary>
        public string Detail { get; set; }

        internal Incident(IncidentsCodes code)
        {
            Code = (int)code;
        }
    }
}
