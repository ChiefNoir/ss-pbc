import { Injectable } from '@angular/core';

@Injectable()
export class StorageService {
  private readonly localStorage: Storage = window.localStorage;
  private readonly tokenName: string = 'AuthorizationToken';

  constructor(){
    console.log('ResourcesService');
  }

  public getToken(): string {
    return JSON.parse(this.localStorage.getItem(this.tokenName));
  }

  public saveToken(value: string): void {
    this.localStorage.setItem(this.tokenName, JSON.stringify(value));
  }

  public removeToken(): void {
    this.localStorage.removeItem(this.tokenName);
  }
}
