import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { RequestResult } from '../model/RequestResult';

import { Project } from '../model/Project';
import { News } from '../model/News';
import { Category } from '../model/Category';

import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';


@Injectable()
export class DataService {
    private httpClient: HttpClient;
    private endpoint = environment.apiEndpoint;

    public constructor(http: HttpClient) {
        this.httpClient = http;
      }

      public getNews(): Promise<RequestResult<Array<News>>> {
        return this.httpClient.get<RequestResult<Array<News>>>
        (
          this.endpoint + 'news/all'
        ).toPromise();
      }

      public getProjects(start: number, length: number, categoryCode: string): Promise<RequestResult<Array<Project>>> {
        return this.httpClient.get<RequestResult<Array<Project>>>
        (
          this.endpoint + 'projects/' + categoryCode + '/' + start + '/' + length
        ).toPromise();
      }

      public getProject(code: string): Promise<RequestResult<Project>> {
        return this.httpClient.get<RequestResult<Project>>
        (
          this.endpoint + 'project/' + code
        ).toPromise();
      }

      public getCategories(): Promise<RequestResult<Array<Category>>>
      {
        return this.httpClient.get<RequestResult<Array<Category>>>
        (
          this.endpoint + 'categories/all'
        ).toPromise();
      }

      public getTotalProjects(categoryCode: string): Promise<RequestResult<number>>
      {
        let param = 'projects/';

        if (typeof categoryCode !== 'undefined' && categoryCode) {
          param += categoryCode + '/';
        }

        return this.httpClient.get<RequestResult<number>>
        (
          this.endpoint + param + 'count'
        ).toPromise();
      }

}
