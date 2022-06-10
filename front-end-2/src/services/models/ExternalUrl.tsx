export class ExternalUrl {
  id: number | null;
  displayName: string;
  url: string;
  version: number;

  public constructor(init?:Partial<ExternalUrl>) {
    Object.assign(this, init);
  }
}
