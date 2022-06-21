import { Incident } from "./Incident";

export type ExecutionResult<T> = {
  isSucceed: boolean;
  data: T;
  error: Incident;
}
