import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Category } from '../shared/category.model';
import { Introduction } from '../introduction/introduction.model';
import { Project } from '../shared/project.model';
import { ProjectPreview } from '../projects/project-preview.model';
import { RequestResult } from '../shared/request-result.model';

import { environment } from 'src/environments/environment';

@Injectable()
export class DataService {
  private endpoint = environment.apiEndpoint;

  public constructor(private httpClient: HttpClient) {}

  public getIntroduction(): Promise<RequestResult<Introduction>> {
    return this.httpClient
      .get<RequestResult<Introduction>>(this.endpoint + 'introduction')
      .toPromise();
  }

  public getCategories(): Promise<RequestResult<Array<Category>>> {
    return this.httpClient
      .get<RequestResult<Array<Category>>>(this.endpoint + 'categories')
      .toPromise();
  }

  public getEverythingCategory(): Promise<RequestResult<Category>> {
    return this.httpClient
      .get<RequestResult<Category>>(this.endpoint + 'categories/everything')
      .toPromise();
  }

  public getCategory(id: number): Promise<RequestResult<Category>> {
    return this.httpClient
      .get<RequestResult<Category>>(this.endpoint + 'categories/' + id)
      .toPromise();
  }

  public getProject(code: string): Promise<RequestResult<Project>> {
    // well, it's way easy and faster to parse code from url, so it will be this way
    return this.httpClient
      .get<RequestResult<Project>>(this.endpoint + 'projects/' + code)
      .toPromise();
  }

  public getProjectsPreview(
    start: number,
    length: number,
    categoryCode: string
  ): Promise<RequestResult<Array<ProjectPreview>>> {
    const categoryParam =
      typeof categoryCode !== 'undefined' && categoryCode
        ? '&categorycode=' + categoryCode
        : '';

    return this.httpClient
      .get<RequestResult<Array<ProjectPreview>>>(
        this.endpoint +
          'projects/search?start=' +
          start +
          '&length=' +
          length +
          categoryParam
      )
      .toPromise();
  }
}
