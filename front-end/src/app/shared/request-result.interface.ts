import { Incident } from './incident.interface'

export interface RequestResult<T> {
  data: T;
  isSucceed: boolean;
  error: Incident;
}
