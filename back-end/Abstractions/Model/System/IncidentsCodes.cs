namespace Abstractions.Model.System
{
    public enum IncidentsCodes
    {
        AuthenticationNotProvided = 100,
        AuthenticationFailed = 101,
        BadToken = 102,

        NotEnoughRights = 103,

        AccessDenied = 200,

        
        InternalError = 300,

        VersionInconsistency = 400
    }
}
