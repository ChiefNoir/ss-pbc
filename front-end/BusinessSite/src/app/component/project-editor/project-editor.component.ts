import { Component, Input, AfterContentInit, OnInit } from '@angular/core';
import { Project } from 'src/app/model/Project';
import { BehaviorSubject } from 'rxjs';
import { DataService } from 'src/app/service/data.service';
import { RequestResult } from 'src/app/model/RequestResult';
import { Category } from 'src/app/model/Category';
import { FormControl, Validators, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-project-editor',
  templateUrl: './project-editor.component.html',
  styleUrls: ['./project-editor.component.scss']
})

export class ProjectEditorComponent implements AfterContentInit, OnInit
{

  private service: DataService;

  @Input()
  public code: string;

  public project$: BehaviorSubject<Project> = new BehaviorSubject<Project>(null);
  public categories$: BehaviorSubject<Category[]> = new BehaviorSubject<Category[]>(null);
  public defaultCategory: Category;


  
  constructor(service: DataService){
    this.service = service;

    
  }

  ngOnInit() {

  }




  ngAfterContentInit(): void {
    if (this.code)
    {
      this.service.getCategories().then
      (
        x => {
          this.categories$.next(x.data.filter(x=>x.isEverything === false));
      this.defaultCategory = x.data.filter(x=>x.isEverything === false)[0];

          this.service.getProject(this.code).then
                (
                  (x) => this.handle(x, this.project$)
                );
        }
      );
    }
    else
    {
      this.service.getCategories().then
      (
        x => {
          this.categories$.next(x.data.filter(x=>x.isEverything === false));
      this.defaultCategory = x.data.filter(x=>x.isEverything === false)[0];

          let prj =new Project();
          prj.category = this.defaultCategory;
          this.project$.next(prj);
        }
      );

    }
  }

  private handle<T>(data: RequestResult<T>, content: BehaviorSubject<T>): void {
    // TODO: handle internal error
    content.next(data.data);
  }


  public close(): void {

  }

  public save(): void {

    this.service.saveProject(this.project$.value)
    .then
    (
      succeeded =>
      {
        //this.message$.next({text: 'Saving complete', type: MessageType.Info  }); 
        this.handle(succeeded, this.project$);
      },
      // /rejected => this.handleError(rejected.message)
    );
  }
}
