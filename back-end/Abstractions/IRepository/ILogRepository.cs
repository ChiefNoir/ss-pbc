using System;

namespace Abstractions.IRepository
{
    /// <summary> Logger </summary>
    public interface ILogRepository
    {
        /// <summary> Log exception </summary>
        /// <param name="exception">Exception to log</param>
        void LogError(Exception exception);
    }
}
