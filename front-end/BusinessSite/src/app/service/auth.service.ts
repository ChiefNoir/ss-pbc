import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { RequestResult } from '../model/RequestResult';
import { Identity } from '../model/Identity';

@Injectable()
export class AuthService
{
  private httpClient: HttpClient;
  private endpoint = environment.authEndpoint;

  public constructor(http: HttpClient)
  {
    this.httpClient = http;
  }

  public login(login: string, password: string): Promise<RequestResult<Identity>>
  {
    const config = { headers: new HttpHeaders().set('Content-Type', 'application/json') };

    return this.httpClient.post<RequestResult<Identity>>
                          (
                              this.endpoint + 'login',
                              { login, password },
                              config
                          )
                          .toPromise();
  }

  public async validate(token: string): Promise<RequestResult<Identity>>
  {
    const headers = new HttpHeaders
    ({
      'Content-Type': 'application/json',
      'Token': token
    });

    return this.httpClient.post<RequestResult<Identity>>
                          (
                              this.endpoint + 'token',
                              null,
                              { headers }
                          )
                          .toPromise();
  }


}
