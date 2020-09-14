using Abstractions.Model.System;
using System;

namespace Abstractions.IFactory
{
    /// <summary> Exceptional situation messages factory </summary>
    public static class IncidentFactory
    {
        /// <summary> Create human-friendly exceptional situation message </summary>
        /// <param name="code"> Exceptional situation code </param>
        /// <param name="details"> Additional message </param>
        /// <returns> <see cref="Incident"/> </returns>
        public static Incident Create(IncidentsCodes code, string details = null)
        {
            var result = new Incident(code)
            {
                Detail = details
            };

            switch (code)
            {
                case IncidentsCodes.AuthenticationNotProvided:
                    {
                        result.Message = "Authentication not provided";
                        break;
                    }
                case IncidentsCodes.AuthenticationFailed:
                    {
                        result.Message = "Authentication failed";
                        break;
                    }
                case IncidentsCodes.AccessDenied:
                    {
                        result.Message = "Access denied";
                        break;
                    }
                case IncidentsCodes.InternalError:
                    {
                        result.Message = "Internal error";
                        break;
                    }
                case IncidentsCodes.VersionInconsistency:
                    {
                        result.Message = "The entry has been already changed";
                        break;
                    }
                case IncidentsCodes.InvalidToken:
                    {
                        result.Message = "Invalid token";
                        break;
                    }
                default:
                    {
                        throw new Exception($"{code} is unknown incident code");
                    }
            }

            return result;
        }
    }
}
