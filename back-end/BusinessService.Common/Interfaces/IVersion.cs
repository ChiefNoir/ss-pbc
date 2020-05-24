namespace BusinessService.Common.Interfaces
{
    /// <summary> Entity with a version </summary>
    public interface IVersion
    {
        /// <summary>Entity version</summary>
        int Version { get; set; }
    }
}
