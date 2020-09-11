import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { RequestResult } from '../model/RequestResult';
import { Project } from '../model/Project';
import { Category } from '../model/Category';
import { Account } from '../model/Account';
import { ProjectPreview } from '../model/ProjectPreview';
import { Introduction } from '../model/Introduction';
import { Information } from '../model/Information';

import { StorageService } from './storage.service';

import { environment } from 'src/environments/environment';

@Injectable()
export class DataService
{
  private httpClient: HttpClient;
  private storage: StorageService;
  private endpoint = environment.apiEndpoint;

  public constructor(http: HttpClient, storage: StorageService)
  {
    this.httpClient = http;
    this.storage = storage;
  }

// --------------------------------------------------------------------
// SWITCH METHODS
  public saveAccount(account: Account): Promise<RequestResult<Account>>
  {
    if (account.id)
    {
      return this.updateAccount(account);
    }
    else
    {
      return this.createAccount(account);
    }
  }

  public saveProject(project: Project): Promise<RequestResult<Project>>
  {
    if (project.id)
    {
      return this.updateProject(project);
    }
    else
    {
      return this.createProject(project);
    }
  }
// [END OF] SWITCH METHODS
// --------------------------------------------------------------------

// --------------------------------------------------------------------
// Restricted methods
  public getInformation(): Promise<RequestResult<Information>>
  {
    const headers = new HttpHeaders
    ({
      'Content-Type': 'application/json',
      Token: this.storage.getToken()
    });

    return this.httpClient
               .get<RequestResult<Information>>
               (
                 this.endpoint + 'information',
                 { headers }
               )
               .toPromise();
  }

  public updateIntroduction(introduction: Introduction): Promise<RequestResult<Introduction>>
  {
    const headers = new HttpHeaders
    ({
      Token: this.storage.getToken()
    });

    const formData = new FormData();
    this.fillFormData(formData, 'project', introduction);

    return this.httpClient
               .patch<RequestResult<Introduction>>
               (
                 this.endpoint + 'introduction',
                 formData,
                 { headers }
               )
               .toPromise();
  }

  public saveCategory(category: Category): Promise<RequestResult<Category>>
  {
    const headers = new HttpHeaders
    ({
      'Content-Type': 'application/json',
      Token: this.storage.getToken()
    });

    return this.httpClient
               .post<RequestResult<Category>>
               (
                 this.endpoint + 'category',
                 category,
                 { headers }
               )
               .toPromise();
  }

  public deleteCategory(category: Category): Promise<RequestResult<any>>
  {
    const headers = new HttpHeaders
    ({
      'Content-Type': 'application/json',
      Token: this.storage.getToken()
    });

    return this.httpClient
               .request<RequestResult<any>>
               (
                 'delete',
                 this.endpoint + 'category',
                 { body: category, headers}
               )
               .toPromise();
  }

  public countAccount(): Promise<RequestResult<number>>
  {
    const headers = new HttpHeaders
    ({
      'Content-Type': 'application/json',
      Token: this.storage.getToken()
    });

    return this.httpClient
               .get<RequestResult<number>>
               (
                 this.endpoint + 'accounts',
                 {headers}
               )
               .toPromise();
  }

  public getAccount(id: number): Promise<RequestResult<Account>>
  {
    const headers = new HttpHeaders
    ({
      'Content-Type': 'application/json',
      Token: this.storage.getToken()
    });

    return this.httpClient
               .get<RequestResult<Account>>
               (
                 this.endpoint + 'accounts/' + id,
                 {headers}
               )
               .toPromise();
  }

  public getRoles(): Promise<RequestResult<string[]>>
  {
    const headers = new HttpHeaders
    ({
      'Content-Type': 'application/json',
      Token: this.storage.getToken()
    });

    return this.httpClient
               .get<RequestResult<string[]>>
               (
                 this.endpoint + 'roles',
                 { headers }
               )
               .toPromise();
  }

  public getAccounts(start: number, length: number): Promise<RequestResult<Account[]>>
  {
    const headers = new HttpHeaders
    ({
      'Content-Type': 'application/json',
      Token: this.storage.getToken()
    });

    return this.httpClient
               .get<RequestResult<Account[]>>
               (
                 this.endpoint + 'accounts/search?start=' + start + '&length=' + length,
                 { headers }
               )
               .toPromise();
  }

  private createAccount(account: Account): Promise<RequestResult<Account>>
  {
    const headers = new HttpHeaders
    ({
      'Content-Type': 'application/json',
      Token: this.storage.getToken()
    });

    return this.httpClient
               .post<RequestResult<Account>>
               (
                 this.endpoint + 'accounts',
                 account,
                 { headers }
               )
               .toPromise();
  }

  private updateAccount(account: Account): Promise<RequestResult<Account>>
  {
    const headers = new HttpHeaders
    ({
      'Content-Type': 'application/json',
      Token: this.storage.getToken()
    });

    return this.httpClient
               .request<RequestResult<Account>>
               (
                'patch',
                 this.endpoint + 'accounts',
                 {body: account, headers}
               )
               .toPromise();
  }

  public deleteAccount(account: Account): Promise<RequestResult<any>>
  {
    const headers = new HttpHeaders
    ({
      'Content-Type': 'application/json',
      Token: this.storage.getToken()
    });

    return this.httpClient
               .request<RequestResult<boolean>>
               (
                 'delete',
                 this.endpoint + 'accounts',
                 { body: account, headers}
               )
               .toPromise();
  }

  public deleteProject(project: Project): Promise<RequestResult<any>>
  {
    const headers = new HttpHeaders
    ({
      'Content-Type': 'application/json',
      Token: this.storage.getToken()
    });

    return this.httpClient
               .request<RequestResult<boolean>>
               (
                 'delete',
                 this.endpoint + 'project',
                 { body: project, headers}
               )
               .toPromise();
  }

  private updateProject(project: Project): Promise<RequestResult<Project>>
  {
    const headers = new HttpHeaders
    ({
      Token: this.storage.getToken()
    });

    const formData = new FormData();
    this.fillFormData(formData, 'project', project);

    return this.httpClient
               .patch<RequestResult<Project>>
               (
                 this.endpoint + 'project',
                 formData,
                 {headers}
               )
               .toPromise();
  }

  private createProject(project: Project): Promise<RequestResult<Project>>
  {
    const headers = new HttpHeaders
    ({
      Token: this.storage.getToken()
    });

    const formData = new FormData();
    this.fillFormData(formData, 'project', project);

    return this.httpClient
               .post<RequestResult<Project>>
               (
                 this.endpoint + 'project',
                 formData,
                 {headers}
               )
               .toPromise();
  }


// [END OF] Restricted methods
// --------------------------------------------------------------------

// --------------------------------------------------------------------
// Public methods
  public getIntroduction(): Promise<RequestResult<Introduction>>
  {
    return this.httpClient
               .get<RequestResult<Introduction>>
               (
                 this.endpoint + 'introduction'
               )
               .toPromise();
  }

  public getCategories(): Promise<RequestResult<Array<Category>>>
  {
    return this.httpClient
              .get<RequestResult<Array<Category>>>
              (
                this.endpoint + 'categories'
              )
              .toPromise();
  }

  public getEverythingCategory(): Promise<RequestResult<Category>>
  {
    return this.httpClient
                .get<RequestResult<Category>>
                (
                  this.endpoint + 'categories/everything'
                )
                .toPromise();
  }

  public getCategory(id: number): Promise<RequestResult<Category>>
  {
    return this.httpClient
               .get<RequestResult<Category>>
               (
                 this.endpoint + 'categories/' + id
               )
               .toPromise();
  }

  public getProject(code: string): Promise<RequestResult<Project>>
  {
    // well, it's way easy and faster to parse code from url, so it will be this way
    return this.httpClient
               .get<RequestResult<Project>>
               (
                 this.endpoint + 'projects/' + code
               )
               .toPromise();
  }

  public getProjectsPreview(start: number, length: number, categoryCode: string): Promise<RequestResult<Array<ProjectPreview>>>
  {
    const categoryParam = typeof categoryCode !== 'undefined' && categoryCode ? '&categorycode=' + categoryCode : '';

    return this.httpClient
              .get<RequestResult<Array<ProjectPreview>>>
              (
                this.endpoint + 'projects/search?start=' + start + '&length=' + length + categoryParam
              )
              .toPromise();
  }

// [END OF] Public methods
// --------------------------------------------------------------------

// --------------------------------------------------------------------
// Helpers methods
  // Usage: this.fillFormData(formData, 'project', project);
  private fillFormData(form: FormData, namespace: string, item: any): void
  {
    for (const property in item)
    {
      if (typeof(item[property]) === 'object')
      {
        if (Array.isArray(item[property]))
        {
          let index = 0;
          item[property].forEach((element: any) => {
            this.fillFormData(form, `${namespace}[${property}][${index}]`, element);
            index++;
          });
        }
        // W3.org: 'A File object is a Blob object with a name attribute, which is a string;'
        else if (item[property] && typeof item[property].name === 'string')
        {
          form.append(`${namespace}[${property}]`, item[property], item[property].name);
        }
        else
        {
          this.fillFormData(form, namespace + `[${property}]`, item[property]);
        }
      }
      else
      {
        form.append(namespace + `[${property}]` , item[property]);
      }
    }
  }

//

}
