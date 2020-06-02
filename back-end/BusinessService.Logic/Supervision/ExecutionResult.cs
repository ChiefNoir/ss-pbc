namespace BusinessService.Logic.Supervision
{
    /// <summary> Result of the function execution</summary>
    public class ExecutionResult
    {
        /// <summary> Is execution succeed?</summary>
        public bool IsSucceed { get; set; } = false;

        /// <summary> Error message or null </summary>
        public string ErrorMessage { get; set; } = null;
    }

    /// <summary> Result of the function execution</summary>
    /// <typeparam name="T">Type of the result</typeparam>
    public class ExecutionResult<T> : ExecutionResult
    {
        /// <summary>Result </summary>
        public T Data { get; set; } = default;
    }
}
