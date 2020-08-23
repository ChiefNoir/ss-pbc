namespace Abstractions.Model.System
{
    public enum IncidentsCodes
    {
        AuthenticationNotProvided = 100,
        AuthenticationFailed = 101,
        BadToken = 102,

        AccessDenied = 200,

        
        InternalError = 300,

        VersionInconsistency = 400
    }
}
