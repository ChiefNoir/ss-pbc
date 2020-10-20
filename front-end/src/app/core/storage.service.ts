import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';

@Injectable()
export class StorageService {
  private readonly tokenName = window.location.hostname + '_token';
  private readonly path = window.location.host;

  public constructor(private cookieService: CookieService) {}

  public getToken(): string {
    return this.cookieService.get(this.tokenName);
  }

  public saveToken(value: string, expires: number): void {
    this.cookieService.set(
      this.tokenName,
      value,
      expires,
      this.path,
      window.location.hostname,
      true,
      'Strict'
    );
  }

  public removeToken(): void {
    this.cookieService.delete(this.tokenName);
  }
}
