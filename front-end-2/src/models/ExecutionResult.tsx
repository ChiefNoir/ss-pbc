import { Incident } from './_index'

type ExecutionResult<T> =
{
    isSucceed: boolean;
    data: T;
    error: Incident;
}

export default ExecutionResult;