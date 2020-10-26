import { HttpClient, HttpHeaders } from '@angular/common/http';
import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { Account } from './account.model';
import { Category } from '../shared/category.model';
import { Information } from './information.interface';
import { Introduction } from '../introduction/introduction.model';
import { Project } from '../shared/project.model';
import { RequestResult } from '../shared/request-result.interface';
import { environment } from 'src/environments/environment';
import { StorageService } from '../core/services/storage.service';
import { DatePipe } from '@angular/common';
import { Observable, throwError } from 'rxjs';
import { Router } from '@angular/router';

@Injectable()
export class PrivateService implements HttpInterceptor  {
  private endpoint = environment.apiEndpoint;

  public constructor(
    private httpClient: HttpClient,
    private storage: StorageService,
    private datepipe: DatePipe,
    private router: Router
  ) {
  }

  // NOTE: https://stackoverflow.com/questions/34934009/handling-401s-globally-with-angular
  // nice way to handle 401 and 403
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((err: HttpErrorResponse) => {
        if (err.status == 401 || err.status == 403) {
          this.storage.removeToken();
          this.router.navigate(['/login']);
        } else {
          return throwError(err);
        }
      })
    );
  }

  public getInformation(): Observable<RequestResult<Information>> {
    const headers = new HttpHeaders(
      {Authorization: `Bearer ${this.storage.getToken()}`
   });

    return this.httpClient
      .get<RequestResult<Information>>(this.endpoint + 'information', {
        headers,
      });
  }

  public saveIntroduction(introduction: Introduction): Observable<RequestResult<Introduction>> {
    const headers = new HttpHeaders(
      {Authorization: `Bearer ${this.storage.getToken()}`
   });

    const formData = new FormData();
    this.fillFormData(formData, 'introduction', introduction);

    return this.httpClient
      .post<RequestResult<Introduction>>(
        this.endpoint + 'introduction',
        formData,
        { headers }
      );
  }

  public saveCategory(category: Category): Observable<RequestResult<Category>> {
    const headers = new HttpHeaders(
      {Authorization: `Bearer ${this.storage.getToken()}`
   });

    return this.httpClient
      .post<RequestResult<Category>>(this.endpoint + 'category', category, {
        headers,
      });
  }

  public deleteCategory(category: Category): Observable<RequestResult<boolean>> {
    const headers = new HttpHeaders(
      {Authorization: `Bearer ${this.storage.getToken()}`
   });

    return this.httpClient
      .request<RequestResult<boolean>>('delete', this.endpoint + 'category', {
        body: category,
        headers,
      });
  }

  public countAccount(): Observable<RequestResult<number>> {
    const headers = new HttpHeaders(
      {Authorization: `Bearer ${this.storage.getToken()}`
   });

    return this.httpClient
      .get<RequestResult<number>>(this.endpoint + 'accounts', { headers });
  }

  public getAccount(id: number): Observable<RequestResult<Account>> {
    const headers = new HttpHeaders(
      {Authorization: `Bearer ${this.storage.getToken()}`
   });

    return this.httpClient
      .get<RequestResult<Account>>(this.endpoint + 'accounts/' + id, {
        headers,
      });
  }

  public getRoles(): Observable<RequestResult<string[]>> {
    const headers = new HttpHeaders(
      {Authorization: `Bearer ${this.storage.getToken()}`
   });

    return this.httpClient
      .get<RequestResult<string[]>>(this.endpoint + 'roles', { headers });
  }

  public getAccounts(start: number, length: number): Observable<RequestResult<Account[]>> {
    const headers = new HttpHeaders(
      {Authorization: `Bearer ${this.storage.getToken()}`
   });

    return this.httpClient
      .get<RequestResult<Account[]>>(
        this.endpoint + 'accounts/search?start=' + start + '&length=' + length,
        { headers }
      );
  }

  public saveAccount(account: Account): Observable<RequestResult<Account>> {
    const headers = new HttpHeaders(
      {Authorization: `Bearer ${this.storage.getToken()}`
   });

    return this.httpClient
      .post<RequestResult<Account>>(this.endpoint + 'accounts', account, {
        headers,
      });
  }

  public deleteAccount(account: Account): Observable<RequestResult<boolean>> {
    const headers = new HttpHeaders(
      {Authorization: `Bearer ${this.storage.getToken()}`
   });

    return this.httpClient
      .request<RequestResult<boolean>>('delete', this.endpoint + 'accounts', {
        body: account,
        headers,
      });
  }

  public deleteProject(project: Project): Observable<RequestResult<boolean>> {
    const headers = new HttpHeaders(
      {Authorization: `Bearer ${this.storage.getToken()}`
   });

    return this.httpClient
      .request<RequestResult<boolean>>('delete', this.endpoint + 'project', {
        body: project,
        headers,
      });
  }

  public saveProject(project: Project): Observable<RequestResult<Project>> {
    const headers = new HttpHeaders(
      {Authorization: `Bearer ${this.storage.getToken()}`
   });

    const formData = new FormData();
    this.fillFormData(formData, 'project', project);

    return this.httpClient
      .post<RequestResult<Project>>(this.endpoint + 'project', formData, {
        headers,
      });
  }

  // Usage: this.fillFormData(formData, 'project', project);
  private fillFormData(form: FormData, namespace: string, item: any): void {
    for (const property in item) {
      if (typeof item[property] === 'object') {
        if (item[property] instanceof Date) {
          form.append(
            namespace + `[${property}]`,
            this.datepipe.transform(item[property], 'yyyy/MM/dd')
          );
        } else if (Array.isArray(item[property])) {
          let index = 0;
          item[property].forEach((element: any) => {
            this.fillFormData(
              form,
              `${namespace}[${property}][${index}]`,
              element
            );
            index++;
          });
        }
        // W3.org: 'A File object is a Blob object with a name attribute, which is a string;'
        else if (item[property] && typeof item[property].name === 'string') {
          form.append(
            `${namespace}[${property}]`,
            item[property],
            item[property].name
          );
        } else {
          this.fillFormData(form, namespace + `[${property}]`, item[property]);
        }
      } else {
        form.append(namespace + `[${property}]`, item[property]);
      }
    }
  }
}
