import { Component, Input, AfterViewInit } from '@angular/core';
import { Project } from 'src/app/model/Project';
import { BehaviorSubject } from 'rxjs';
import { DataService } from 'src/app/service/data.service';
import { RequestResult } from 'src/app/model/RequestResult';

@Component({
  selector: 'app-project-editor',
  templateUrl: './project-editor.component.html',
  styleUrls: ['./project-editor.component.scss']
})

export class ProjectEditorComponent implements AfterViewInit
{

  private service: DataService;
  @Input()
  public code: string;
  
  public project$: BehaviorSubject<Project> = new BehaviorSubject<Project>(null);

  constructor(service: DataService){
    this.service = service;
  }

  ngAfterViewInit(): void {
    this.service.getProject(this.code).then
    (
      (x) => this.handle(x, this.project$)
    );
  }

  private handle<T>(data: RequestResult<T>, content: BehaviorSubject<T>): void {
    // TODO: handle internal error
    content.next(data.data);
  }
}
