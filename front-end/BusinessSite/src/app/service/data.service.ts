import { HttpClient, HttpEvent } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { RequestResult } from '../model/RequestResult';
import { Project } from '../model/Project';
import { News } from '../model/News';
import { Category } from '../model/Category';
import { Account } from '../model/Account';

import { environment } from 'src/environments/environment';
import { ProjectPreview } from '../model/ProjectPreview';
import { share } from 'rxjs/operators';
import { Observable } from 'rxjs';

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
          'projects/search?' +
          'start=' +
          start +
          '&length=' +
          length +
          categoryParam
      )
      .toPromise();
  }

  public getProject(code: string): Promise<RequestResult<Project>> {
    return this.httpClient
      .get<RequestResult<Project>>(this.endpoint + 'project/' + code)
      .toPromise();
  }

  public getCategories(): Promise<RequestResult<Array<Category>>> {
    return this.httpClient
      .get<RequestResult<Array<Category>>>(this.endpoint + 'categories/all')
      .toPromise();
  }

  public getEverythingCategory(): Promise<RequestResult<Category>> {
    return this.httpClient
      .get<RequestResult<Category>>(this.endpoint + 'category/everything')
      .toPromise();
  }

  public getCategory(code: string): Promise<RequestResult<Category>> {
    return this.httpClient
      .get<RequestResult<Category>>(this.endpoint + 'category/' + code)
      .toPromise();
  }

  public save(category: Category): Promise<RequestResult<any>> {
    return this.httpClient
      .post<RequestResult<any>>(this.endpoint + 'category', category)
      .toPromise();
  }

  public delete(category: Category): Promise<RequestResult<any>> {
    return this.httpClient
      .request<RequestResult<any>>('delete', this.endpoint + 'category', {
        body: category,
      })

      .toPromise();
  }

  public saveProject(project: Project): Promise<RequestResult<any>> {
    return this.httpClient
      .post<RequestResult<any>>(this.endpoint + 'project', project)
      .toPromise();
  }

  public deleteProject(project: Project): Promise<RequestResult<any>> {
    return this.httpClient
      .request<RequestResult<any>>('delete', this.endpoint + 'project', {
        body: project,
      })

      .toPromise();
  }


  public uploadFile(fileToUpload: File): Promise<RequestResult<string>>{
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);

    return this.httpClient.post<RequestResult<string>>(this.endpoint + 'upload', formData).toPromise();
  }


  public countAccount(): Promise<RequestResult<number>> {
    return this.httpClient
      .get<RequestResult<number>>(this.endpoint + 'accounts')
      .toPromise();
  }

  public getAccount(id: number): Promise<RequestResult<Account>>
  {
    return this.httpClient
    .get<RequestResult<Account>>(
      this.endpoint +
        'accounts/' + id
    )
    .toPromise();
  }

  public getAccounts(start: number, length: number): Promise<RequestResult<Account[]>>
  {
    return this.httpClient
    .get<RequestResult<Account[]>>(
      this.endpoint +
        'accounts/search?' +
        'start=' +
        start +
        '&length=' +
        length
    )
    .toPromise();
  }

  public saveAccount(account: Account): Promise<RequestResult<Account>> {
    return this.httpClient
      .post<RequestResult<Account>>(this.endpoint + 'account', account)
      .toPromise();
  }
}
