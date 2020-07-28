namespace Abstractions.Common
{
    /// <summary> Entity with a version </summary>
    public interface IVersion
    {
        /// <summary> Entity version </summary>
        long Version { get; }
    }
}