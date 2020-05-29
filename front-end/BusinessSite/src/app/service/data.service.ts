import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { RequestResult } from '../model/RequestResult';
import { Project } from '../model/Project';

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

      public getProjects(start: number, length: number, categoryCode: string): Observable<RequestResult<Array<Project>>> {
        return this.httpClient.get<RequestResult<Array<Project>>>
        (
          this.endpoint + 'projects/' + categoryCode + '/' + start + '/' + length + '/'
        );
      }
}
