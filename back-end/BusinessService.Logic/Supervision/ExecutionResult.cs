namespace BusinessService.Logic.Supervision
{
    /// <summary> Result of save execution</summary>
    /// <typeparam name="T">Type</typeparam>
    public class ExecutionResult<T>
    {
        /// <summary>Result </summary>
        public T Data { get; internal set; } = default;

        /// <summary> Is execution succeed?</summary>
        public bool IsSucceed { get; internal set; } = false;

        /// <summary> Error message or null </summary>
        public string ErrorMessage { get; internal set; } = null;
    }
}
