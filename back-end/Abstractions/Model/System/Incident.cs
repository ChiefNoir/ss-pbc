namespace Abstractions.Model.System
{
    public sealed class Incident
    {
        public int Code { get; private set; }
        public string Message { get; internal set; }
        public string Detail { get; set; }

        internal Incident(IncidentsCodes code)
        {
            Code = (int)code;
        }
    }
}
