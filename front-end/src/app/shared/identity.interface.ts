import { Account } from '../admin/account.model';

export interface Identity {
  account: Account;
  token: string;
  tokenLifeTimeMinutes: number;
}
