import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { RequestResult } from '../model/RequestResult';

@Injectable()
export class AuthService {
  private httpClient: HttpClient;
  private endpoint = environment.authEndpoint;



  public constructor(http: HttpClient) {
    this.httpClient = http;
  }

  public login(login: string, password: string): Promise<RequestResult<boolean>> {
    const config = { headers: new HttpHeaders().set('Content-Type', 'application/json') };
    return this.httpClient
               .post<RequestResult<boolean>>(this.endpoint + 'login', {
                login,
                password
              }, config)
               .toPromise();
  }
}