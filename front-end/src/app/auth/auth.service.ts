import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Identity } from '../shared/models/identity.interface';
import { RequestResult } from '../shared/models/request-result.interface';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';

@Injectable()
export class AuthService {
  private endpoint = environment.authEndpoint;

  public constructor(private httpClient: HttpClient) {}

  public login(login: string, password: string): Observable<RequestResult<Identity>> {
    return this.httpClient
               .post<RequestResult<Identity>>(
                 this.endpoint + 'login', { login, password}
               );
  }
}
