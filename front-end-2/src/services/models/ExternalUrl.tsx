export class ExternalUrl {
  id: string | null;
  displayName: string;
  url: string;
  version: number;

  public constructor(init?:Partial<ExternalUrl>) {
    Object.assign(this, init);
  }
}
