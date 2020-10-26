import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Identity } from '../shared/identity.interface';
import { RequestResult } from '../shared/request-result.model';
import { environment } from 'src/environments/environment';

@Injectable()
export class AuthService {
  private endpoint = environment.authEndpoint;

  public constructor(private httpClient: HttpClient) {}

  public login(login: string, password: string): Promise<RequestResult<Identity>> {
    return this.httpClient
      .post<RequestResult<Identity>>(this.endpoint + 'login', {
        login,
        password,
      })
      .toPromise();
  }

}
