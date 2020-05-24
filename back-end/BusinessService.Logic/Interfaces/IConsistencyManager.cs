using BusinessService.Common.Interfaces;

namespace BusinessService.Logic.Interfaces
{
    /// <summary> Consistency Manager will validate <see cref="IVersion"/> entity on update action
    /// If <see cref="int"/> Version of the entity is different, then the update action is invalid and must be stopped.</summary>
    public interface IConsistencyManager
    {
        /// <summary> Validate <typeparamref name="T"/> entity</summary>
        /// <typeparam name="T">Class of the <seealso cref="IVersion"/> entity</typeparam>
        /// <param name="key">[Key] field of the entity</param>
        /// <param name="item">Database entity to validate before database update</param>
        void ValidateBeforeUpdate<T>(object key, T entity) where T : class, IVersion;
    }
}
