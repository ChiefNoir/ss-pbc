import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { BehaviorSubject } from 'rxjs';

import { DataService } from 'src/app/service/data.service';
import { ProjectPreview } from 'src/app/model/ProjectPreview';
import { Category } from 'src/app/model/Category';

import { environment } from 'src/environments/environment';
import { MessageDescription, MessageType } from 'src/app/component/message/message.component';
import { StaticNames } from 'src/app/common/StaticNames';
import { Paging } from 'src/app/model/PagingInfo';

@Component({
  selector: 'app-projects-list',
  templateUrl: './projects-list.component.html',
  styleUrls: ['./projects-list.component.scss'],
})

export class ProjectsListComponent implements OnDestroy, OnInit
{
  private service: DataService;
  private router: Router;
  private activeRoute: ActivatedRoute;

  public projects$: BehaviorSubject<Array<ProjectPreview>> = new BehaviorSubject<Array<ProjectPreview>>(null);
  public paging$: BehaviorSubject<Paging<string>> = new BehaviorSubject<Paging<string>>(null);
  public categories$: BehaviorSubject<Array<Category>> = new BehaviorSubject<Array<Category>>(null);
  public message$: BehaviorSubject<MessageDescription> = new BehaviorSubject<MessageDescription>({text: StaticNames.LoadInProgress, type: MessageType.Spinner });

  public constructor(service: DataService, router: Router, activeRoute: ActivatedRoute, titleService: Title)
  {
    this.service = service;
    this.router = router;
    this.activeRoute = activeRoute;

    titleService.setTitle(environment.siteName + ' - Projects');
  }

  public ngOnInit(): void
  {
    this.activeRoute.params.subscribe(() => { this.refreshPage(); });
    this.paging$.subscribe(value => this.refreshProjects(value));
  }

  public ngOnDestroy(): void
  {
    this.paging$.unsubscribe();
  }


  private refreshPage(): void
  {
    this.projects$.next(null);
    this.categories$.next(null);

    this.service.getCategories()
                .then
                (
                  response => this.refreshCategories(response.data),
                  reject => this.handleError(reject)
                );
  }

  private refreshCategories(categories: Category[])
  {
    const routeCategory = this.activeRoute.snapshot.paramMap.get('category');

    const selectedCategory = categories.find((x) => x.code === routeCategory);
    const everythingCategory = categories.find((x) => x.isEverything === true);

    if (!routeCategory)
    {
      this.router.navigate(['/projects/' + everythingCategory.code]);
      return;
    }

    if (!selectedCategory)
    {
      this.router.navigate(['/404/']);
      return;
    }

    this.categories$.next(categories);
    this.paging$.next(new Paging(0, environment.paging.maxProjects, selectedCategory.totalProjects, selectedCategory.code));
  }

  private refreshProjects(paging: Paging<string>): void
  {
    if (!paging) { return; }

    this.service.getProjectsPreview
                (
                  paging.getCurrentPage() * environment.paging.maxProjects,
                  environment.paging.maxProjects,
                  paging.getSearchParam()
                )
                .then
                (
                  response =>
                  {
                    this.message$.next(null);
                    this.projects$.next(response.data);
                  },
                  reject => this.handleError(reject)
                );
  }


  public skipPage(amount: number): void
  {
    this.changePage(this.paging$.value.getCurrentPage() + amount);
  }

  public changePage(page: number): void
  {
    this.paging$.next
        (
          new Paging
              (
                page,
                environment.paging.maxProjects,
                this.paging$.value.getMaxItems(),
                this.paging$.value.getSearchParam()
              )
        );
  }

  private handleError(error: any): void
  {
    this.message$.next({text: error, type: MessageType.Error});
    console.log(error);
  }


}
