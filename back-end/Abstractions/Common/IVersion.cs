namespace Abstractions.Common
{
    /// <summary>Entity with a version</summary>
    public interface IVersion
    {
        /// <summary>Get entity version</summary>
        long Version { get; }
    }
}