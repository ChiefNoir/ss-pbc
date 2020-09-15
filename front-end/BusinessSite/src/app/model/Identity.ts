import { Account } from 'src/app/model/Account';

export class Identity
{
    public account: Account;
    public token: string;
    public tokenLifeTimeMinutes: number;
}
