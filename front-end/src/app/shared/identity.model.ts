import { Account } from 'src/app/admin/account.model';

export class Identity
{
    public account: Account;
    public token: string;
    public tokenLifeTimeMinutes: number;
}
