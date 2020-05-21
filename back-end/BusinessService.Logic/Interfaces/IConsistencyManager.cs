using BusinessService.Common.Interfaces;

namespace BusinessService.Logic.Interfaces
{
    /// <summary> Consistency Manager will validate <see cref="IVersion"/> entity on update action
    /// If Version is different, so the update action is invalid
    /// </summary>
    public interface IConsistencyManager
    {
        void ValidateUpdateAction<T>(object key, T item) where T : class, IVersion;
    }
}
