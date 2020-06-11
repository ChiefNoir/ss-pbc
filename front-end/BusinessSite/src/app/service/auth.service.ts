import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { RequestResult } from '../model/RequestResult';
import { Identity } from '../model/Identity';
import { CookieService } from 'ngx-cookie-service';

@Injectable()
export class AuthService {
  private httpClient: HttpClient;
  private endpoint = environment.authEndpoint;
  private cookieService: CookieService;
  private readonly tokenName = window.location.hostname + '_token';

  public constructor(http: HttpClient, cookieService: CookieService) {
    this.httpClient = http;
    this.cookieService = cookieService;
  }

  public login(login: string, password: string): Promise<RequestResult<Identity>> {
    const config = { headers: new HttpHeaders().set('Content-Type', 'application/json') };
    return this.httpClient
               .post<RequestResult<Identity>>(this.endpoint + 'login', {
                login,
                password
              }, config)
               .toPromise();
  }

  public pingadmin(): Promise<RequestResult<string>> {

    const token = this.cookieService.get(this.tokenName);
    const config = { headers: new HttpHeaders().set('Authorization', token) };

    return this.httpClient
               .get<RequestResult<string>>(this.endpoint + 'pingadmin', config)
               .toPromise();
  }
  public pingdemo(): Promise<RequestResult<string>> {
    const token = this.cookieService.get(this.tokenName);
    const config = { headers: new HttpHeaders().set('Authorization', token) };

    return this.httpClient
               .get<RequestResult<string>>(this.endpoint + 'pingdemo', config)
               .toPromise();
  }
}
