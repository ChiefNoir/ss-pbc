import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Category } from '../../shared/models/category.model';
import { Introduction } from '../../shared/models/introduction.model';
import { Project } from '../../shared/models/project.model';
import { ProjectPreview } from '../../shared/models/project-preview.interface';
import { RequestResult } from '../../shared/models/request-result.interface';

import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class PublicService {
  private endpoint = environment.apiEndpoint;

  public constructor(private httpClient: HttpClient) { }

  public getIntroduction(): Observable<RequestResult<Introduction>> {
    return this.httpClient
      .get<RequestResult<Introduction>>(this.endpoint + 'introduction');
  }

  public getCategories(): Observable<RequestResult<Array<Category>>> {
    return this.httpClient
      .get<RequestResult<Array<Category>>>(this.endpoint + 'categories');
  }

  public getEverythingCategory(): Observable<RequestResult<Category>> {
    return this.httpClient
      .get<RequestResult<Category>>(this.endpoint + 'categories/everything');
  }

  public getCategory(id: number): Observable<RequestResult<Category>> {
    return this.httpClient
      .get<RequestResult<Category>>(this.endpoint + 'categories/' + id);
  }

  public getProject(code: string): Observable<RequestResult<Project>> {
    // well, it's way easy and faster to parse code from url, so it will be this way
    return this.httpClient
      .get<RequestResult<Project>>(this.endpoint + 'projects/' + code);
  }

  public getProjectsPreview(start: number, length: number, categoryCode: string)
    : Observable<RequestResult<Array<ProjectPreview>>> {
    const categoryParam =
      typeof categoryCode !== 'undefined' && categoryCode
        ? '&categorycode=' + categoryCode
        : '';

    return this.httpClient
      .get<RequestResult<Array<ProjectPreview>>>(
        this.endpoint +
          'projects/search?start=' + start +
          '&length=' + length +
          categoryParam
      );
  }
}
