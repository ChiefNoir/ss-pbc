class Account {
  id: number | null;
  login: string;
  password: string;
  role: string;
  version: number;

  public constructor(init?:Partial<Account>) {
    Object.assign(this, init);
  }
}

export { Account };
