import { Account } from "..";

export type Identity = {
  account: Account;
  token: string;
  tokenLifeTimeMinutes: number;
}
