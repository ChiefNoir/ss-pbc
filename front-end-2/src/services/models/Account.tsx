class Account {
  id: number | null = null;
  login: string;
  password: string;
  role: string = "";
  version: number = 0;

  public constructor(init?:Partial<Account>) {
    Object.assign(this, init);
}
}

export { Account };
