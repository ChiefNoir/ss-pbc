import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Identity } from '../shared/identity.model';
import { RequestResult } from '../shared/request-result.model';
import { environment } from 'src/environments/environment';

@Injectable()
export class AuthService
{
  private endpoint = environment.authEndpoint;
  private httpClient: HttpClient;

  public constructor(http: HttpClient)
  {
    this.httpClient = http;
  }

  public login(login: string, password: string): Promise<RequestResult<Identity>>
  {
    return this.httpClient.post<RequestResult<Identity>>
                          (
                              this.endpoint + 'login',
                              { login, password }
                          )
                          .toPromise();
  }

  public async validate(token: string): Promise<RequestResult<Identity>>
  {
    const headers = new HttpHeaders({Token: token});

    return this.httpClient
               .post<RequestResult<Identity>>
               (
                 this.endpoint + 'token',
                 null,
                 { headers }
               )
               .toPromise();
  }
}
