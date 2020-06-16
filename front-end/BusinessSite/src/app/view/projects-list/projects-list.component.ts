import { Component } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { BehaviorSubject } from 'rxjs';

import { DataService } from 'src/app/service/data.service';
import { RequestResult } from 'src/app/model/RequestResult';
import { ProjectPreview } from 'src/app/model/ProjectPreview';
import { Category } from 'src/app/model/Category';
import { PagingInfo } from 'src/app/model/PagingInfo';

import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-projects-list',
  templateUrl: './projects-list.component.html',
  styleUrls: ['./projects-list.component.scss'],
})

export class ProjectsListComponent {
  private service: DataService;
  private router: Router;
  private activeRoute: ActivatedRoute;
  private projectsPerPage = environment.maxProjectsPerPage;

  private currentPage: number = 0;
  private maxPage: number = 0;
  private minPage: number = 0;

  public projects$: BehaviorSubject<Array<ProjectPreview>> = new BehaviorSubject<Array<ProjectPreview>>(null);
  public pagingInfo$: BehaviorSubject<PagingInfo> = new BehaviorSubject<PagingInfo>(null);
  public categories$: BehaviorSubject<Array<Category>> = new BehaviorSubject<Array<Category>>(null);

  public constructor(service: DataService, router: Router, activeRoute: ActivatedRoute, titleService: Title) {
    this.service = service;
    this.router = router;
    this.activeRoute = activeRoute;

    titleService.setTitle(environment.siteName + ' - Projects');

    this.activeRoute.params.subscribe(() => { this.refreshPage(); });
  }

  private refreshPage(): void {
    this.projects$.next(null);
    this.currentPage = 0;
    this.maxPage = 1;

    const categoryCode = this.activeRoute.snapshot.paramMap.get('category');

    if (!categoryCode)
    {
      this.service.getEverythingCategory()
                  .then
                  (
                    (x) => {
                      if (x.isSucceed) {
                        this.router.navigate(['/projects/' + x.data.code]);
                      } else {
                        this.router.navigate(['/404/']);
                      }
                    }
                  );
    }

    if (this.categories$.value == null) {
      this.service.getCategories()
                  .then
                  (
                    (data) => { this.handleCategories(data); }
                  );
    }
    else{
      const categoryCode = this.activeRoute.snapshot.paramMap.get('category');
      const totalProjects = +this.categories$.value.find((x) => x.code === categoryCode)?.totalProjects;
      this.handleTotalProjects(totalProjects);
    }

    

    this.service.getProjectsPreview(this.currentPage * this.projectsPerPage, this.projectsPerPage, categoryCode)
                .then
                  (
                    (data) => { this.handleProjects(data); }
                  );

  }

  private handleProjects(data: RequestResult<Array<ProjectPreview>>): void {
    if (data.isSucceed)
    {
      if (data.data == null || data.data.length === 0)
      {
        alert('?');
        // this.router.navigate(['/404']);
      }
      else
      {
        this.projects$.next(data.data);
      }
    }
    else
    {
      this.handleError(data.errorMessage);
    }
  }

  private handleCategories(result: RequestResult<Array<Category>>): void {
    if (result.isSucceed)
    {
      const router = this.router;
      result.data.forEach
                  (
                    (x) => { x.url = router.createUrlTree(['/projects', x.code]).toString(); }
                  );

      this.categories$.next
                      (
                        result.data
                              .filter((x) => x.totalProjects > 0)
                              .sort((a, b) => b.totalProjects - a.totalProjects)
                      );

      const categoryCode = this.activeRoute.snapshot.paramMap.get('category');
      const totalProjects = !result.data ? 0 : result.data.find((x) => x.code === categoryCode)?.totalProjects;
      this.handleTotalProjects(totalProjects);
    } 
    else 
    {
      this.handleError(result.errorMessage);
    }
  }

  private handleTotalProjects(totalProjects: number): void {
    this.currentPage = 0;
    this.maxPage = Math.ceil(totalProjects / this.projectsPerPage) - 1;

    const pgInfo: PagingInfo = {
      minPage: this.minPage,
      maxPage: this.maxPage,
      currentPage: this.currentPage,
    };

    this.pagingInfo$.next(pgInfo);
  }











  public changePage(page: number): void {
    if (!page) {
      page = this.minPage;
    }
    if (page > this.maxPage) {
      page = this.maxPage;
    }
    if (page < this.minPage) {
      page = this.minPage;
    }

    this.currentPage = page;
    this.projects$.next(null);

    this.pagingInfo$.next
    ({
        minPage: this.minPage,
        maxPage: this.maxPage,
        currentPage: this.currentPage
    });

    const categoryCode = this.activeRoute.snapshot.paramMap.get('category');
    this.service.getProjectsPreview(this.currentPage * environment.maxProjectsPerPage, environment.maxProjectsPerPage, categoryCode)
                .then
                (
                  (result) => this.handleProjects(result),
                  (error) => this.handleError(error)
                );
  }

  public nextPage() {
    this.changePage(this.currentPage + 1);
  }

  public backPage() {
    this.changePage(this.currentPage - 1);
  }

  private handleError(error: any): void {
    // TODO: react properly
    console.log(error);
  }
}
