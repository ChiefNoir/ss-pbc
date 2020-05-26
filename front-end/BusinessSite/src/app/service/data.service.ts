import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { RequestResult } from '../model/RequestResult';
import { News } from '../model/News';

import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable()
export class DataService {
    private httpClient: HttpClient;
    private endpoint = environment.apiEndpoint;

    public constructor(http: HttpClient) {
        this.httpClient = http;
      }

      public getNews(): Observable<RequestResult<Array<News>>> {
        return this.httpClient.get<RequestResult<Array<News>>>
        (
          this.endpoint + 'news/all'
        );
      }

}