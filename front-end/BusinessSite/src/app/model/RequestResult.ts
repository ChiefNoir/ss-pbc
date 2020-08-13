export class RequestResult<T> {
  public data: T;

  public isSucceed: boolean;

  public errorMessage: string;

  public errorCode: number;
}
