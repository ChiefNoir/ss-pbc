export class RequestResult<T>
{
  public data: T;
  public isSucceed: boolean;
  public error: Incident;
}

export class Incident
{
  public message: string;
  public detail: string;
}