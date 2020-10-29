import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { BehaviorSubject } from 'rxjs';

import { PublicService } from '../../core/services/public.service';
import { ProjectPreview } from '../../shared/project-preview.interface';
import { Category } from '../../shared/category.model';
import { Paging } from '../../shared/paging-info.model';
import { RequestResult } from '../../shared/request-result.interface';
import { Incident } from '../../shared/incident.interface';

import { MessageDescription, MessageType } from '../../shared/message/message.component';
import { environment } from 'src/environments/environment';
import { ResourcesService } from '../../core/services/resources.service';

@Component({
  selector: 'app-projects-list',
  templateUrl: './projects-list.component.html',
  styleUrls: ['./projects-list.component.scss'],
})
export class ProjectsListComponent implements OnDestroy, OnInit {
  public categories$: BehaviorSubject<Array<Category>> = new BehaviorSubject<Array<Category>>(null);
  public message$: BehaviorSubject<MessageDescription> = new BehaviorSubject<MessageDescription>({ type: MessageType.Spinner });
  public paging$: BehaviorSubject<Paging<string>> = new BehaviorSubject<Paging<string>>(null);
  public projects$: BehaviorSubject<Array<ProjectPreview>> = new BehaviorSubject<Array<ProjectPreview>>(null);

  public constructor(
    private service: PublicService,
    private router: Router,
    private activeRoute: ActivatedRoute,
    titleService: Title,
    public textMessages: ResourcesService
  ) {
    titleService.setTitle(this.textMessages.TitleProjects + environment.siteName);
  }

  public ngOnInit(): void {
    this.activeRoute.params.subscribe(() => {
      this.refreshPage();
    });

    this.paging$.subscribe((value) => this.refreshProjects(value));
  }

  public ngOnDestroy(): void {
    this.paging$.unsubscribe();
  }

  private refreshPage(): void {
    this.projects$.next(null);
    this.categories$.next(null);

    this.service
        .getCategories()
        .subscribe(
          win => this.handleCategorie(win),
          fail => this.handleError(fail)
    );
  }

  private refreshCategories(categories: Category[]): void {
    const routeCategory = this.activeRoute.snapshot.paramMap.get('category');
    const selectedCategory = categories.find((x) => x.code === routeCategory);
    const everythingCategory = categories.find((x) => x.isEverything === true);

    if (!routeCategory) {
      this.router.navigate(['/projects/' + everythingCategory.code]);
      return;
    }

    if (!selectedCategory) {
      this.router.navigate(['/404/']);
      return;
    }

    this.categories$.next(categories);
    this.paging$.next(
      new Paging(
        0,
        environment.paging.maxProjects,
        selectedCategory.totalProjects,
        selectedCategory.code
      )
    );
  }

  private refreshProjects(paging: Paging<string>): void {
    if (!paging) {
      return;
    }

    this.projects$.next(null);
    this.message$.next({ type: MessageType.Spinner });

    this.service
      .getProjectsPreview(
        paging.getCurrentPage() * environment.paging.maxProjects,
        environment.paging.maxProjects,
        paging.getSearchParam()
      )
      .subscribe(
        win => this.handleProjects(win),
        fail => this.handleError(fail)
      );
  }

  public skipPage(amount: number): void {
    this.changePage(this.paging$.value.getCurrentPage() + amount);
  }

  public changePage(page: number): void {
    this.paging$.next(
      new Paging(
        page,
        environment.paging.maxProjects,
        this.paging$.value.getMaxItems(),
        this.paging$.value.getSearchParam()
      )
    );
  }

  private handleCategorie(response: RequestResult<Category[]>): void {
    if (response.isSucceed) {
      this.refreshCategories(response.data);
    } else {
      this.handleIncident(response.error);
    }
  }

  private handleProjects(response: RequestResult<ProjectPreview[]>): void {
    if (response.isSucceed) {
      this.message$.next(null);
      this.projects$.next(response.data);

      if (response.data.length === 0) {
        this.message$.next({
          text: this.textMessages.ErrorProjectsNotFound,
          type: MessageType.Info,
        });
      }
    } else {
      this.handleIncident(response.error);
    }
  }

  private handleIncident(error: Incident): void {
    this.message$.next({ text: error.message, type: MessageType.Error });
  }

  private handleError(error: any): void {
    if (error.name !== undefined) {
      this.message$.next({ text: error.name, type: MessageType.Error });
    } else {
      this.message$.next({ text: error, type: MessageType.Error });
    }
  }
}
