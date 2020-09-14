namespace Abstractions.Model.System
{
    /// <summary> Exceptional situation codes</summary>
    public enum IncidentsCodes
    {
        AuthenticationNotProvided = 100,
        AuthenticationFailed = 101,
        InvalidToken = 102,

        NotEnoughRights = 103,

        AccessDenied = 200,

        
        InternalError = 300,

        VersionInconsistency = 400
    }
}
