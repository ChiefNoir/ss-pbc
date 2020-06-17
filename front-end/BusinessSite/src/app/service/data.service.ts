import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { RequestResult } from '../model/RequestResult';
import { Project } from '../model/Project';
import { News } from '../model/News';
import { Category } from '../model/Category';

import { environment } from 'src/environments/environment';
import { ProjectPreview } from '../model/ProjectPreview';

@Injectable()
export class DataService {
  private httpClient: HttpClient;
  private endpoint = environment.apiEndpoint;

  public constructor(http: HttpClient) {
    this.httpClient = http;
  }

  public getNews(): Promise<RequestResult<Array<News>>> {
    return this.httpClient
               .get<RequestResult<Array<News>>>(this.endpoint + 'news/all')
               .toPromise();
  }

  public getProjectsPreview(start: number, length: number, categoryCode: string): Promise<RequestResult<Array<ProjectPreview>>> {

    const categoryParam = typeof categoryCode !== 'undefined' && categoryCode ? '&categorycode=' + categoryCode : '';

    return this.httpClient
               .get<RequestResult<Array<ProjectPreview>>>
               (
                 this.endpoint + 'projects/search?'
                 + 'start=' + start + '&length=' + length
                 + categoryParam
               )
               .toPromise();
  }

  public getProject(code: string): Promise<RequestResult<Project>> {
    return this.httpClient
               .get<RequestResult<Project>>
               (
                 this.endpoint + 'project/' + code
               )
               .toPromise();
  }

  public getCategories(): Promise<RequestResult<Array<Category>>> {
    return this.httpClient
               .get<RequestResult<Array<Category>>>
               (
                 this.endpoint + 'categories/all'
               )
               .toPromise();
  }

  public getEverythingCategory(): Promise<RequestResult<Category>> {
    return this.httpClient
               .get<RequestResult<Category>>
               (
                 this.endpoint + 'category/everything'
               ).toPromise();
  }

  public getCategory(code: string): Promise<RequestResult<Category>> {
    return this.httpClient
               .get<RequestResult<Category>>
               (
                 this.endpoint + 'category/' + code
               )
               .toPromise();
  }
}
