using Abstractions.Model.System;
using System;

namespace Abstractions.IFactory
{
    public static class IncidentFactory
    {
        public static Incident Create(IncidentsCodes code)
        {
            var result = new Incident(code);

            switch (code)
            {
                case IncidentsCodes.AuthenticationNotProvided:
                    {
                        result.Message = "AuthenticationNotProvided";
                        break;
                    }
                case IncidentsCodes.AuthenticationFailed:
                    {
                        result.Message = "AuthenticationFailed";
                        break;
                    }
                case IncidentsCodes.AccessDenied:
                    {
                        result.Message = "AccessDenied";
                        break;
                    }
                case IncidentsCodes.InternalError:
                    {
                        result.Message = "InternalError";
                        break;
                    }
                case IncidentsCodes.VersionInconsistency:
                    {
                        result.Message = "VersionInconsistency";
                        break;
                    }
                case IncidentsCodes.BadToken:
                    {
                        result.Message = "BadToken";
                        break;
                    }
                default:
                    {
                        throw new Exception($"{code} is unknown Incident code");
                    }
            }

            return result;
        }
    }
}
