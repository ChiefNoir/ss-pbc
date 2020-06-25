import { Component, Input, AfterContentInit, OnInit, ViewChild } from '@angular/core';
import { Project } from 'src/app/model/Project';
import { BehaviorSubject } from 'rxjs';
import { DataService } from 'src/app/service/data.service';
import { RequestResult } from 'src/app/model/RequestResult';
import { Category } from 'src/app/model/Category';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { ExternalUrl } from 'src/app/model/ExternalUrl';
import { MatTable } from '@angular/material/table';
import { MessageType, MessageDescription } from '../message/message.component';
import { MatDialogRef } from '@angular/material/dialog';
import { HttpEventType } from '@angular/common/http';

@Component({
  selector: 'app-project-editor',
  templateUrl: './project-editor.component.html',
  styleUrls: ['./project-editor.component.scss']
})

export class ProjectEditorComponent implements AfterContentInit, OnInit
{

  public columnsInner: string[] = [ 'name', 'url', 'btn'];
  @ViewChild('externalUrlsTable') externalUrlsTable: MatTable<any>;
  private dialog: MatDialogRef<ProjectEditorComponent>;
  
  private service: DataService;

  @Input()
  public code: string;

  public project$: BehaviorSubject<Project> = new BehaviorSubject<Project>(null);
  public categories$: BehaviorSubject<Category[]> = new BehaviorSubject<Category[]>(null);
  public defaultCategory: Category;


  public disableInput$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  public message$: BehaviorSubject<MessageDescription> = new BehaviorSubject<MessageDescription>(null);

  public loadingMessage: MessageDescription = {text: 'Loading', type: MessageType.Spinner };
  
  constructor(service: DataService, dialog: MatDialogRef<ProjectEditorComponent>){
    this.service = service;
    this.dialog = dialog;
    
  }

  ngOnInit() {

  }




  ngAfterContentInit(): void {
    this.refresh();
  }

  private handle<T>(result: RequestResult<T>, content: BehaviorSubject<T>): void {

    if(result.isSucceed) {
    content.next(result.data);
    this.message$.next(null);
    } else {
      this.message$.next({text: result.errorMessage, type: MessageType.Error  });
    }
  }

  public add(): void {

    if(this.project$.value.externalUrls) {
    this.project$.value.externalUrls.push(new ExternalUrl());

    this.externalUrlsTable.renderRows();
    }
    else {
      this.project$.value.externalUrls = new Array<ExternalUrl>();
      this.project$.value.externalUrls.push(new ExternalUrl());
    }
  }

  public removeUrl(extUrl: ExternalUrl ): void {
    const index = this.project$.value.externalUrls.indexOf(extUrl, 0);
if (index > -1) {
  this.project$.value.externalUrls.splice(index, 1);
}
this.externalUrlsTable.renderRows();
  }

  public close(): void {
    this.dialog.close();
  }

  public refresh(): void {
    
    this.disableInput$.next(true);
    this.message$.next({text: 'Loading', type: MessageType.Spinner  });

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
          this.message$.next(null);
        }
      );

    }
  }

  public delete(): void {
    this.disableInput$.next(true);
    this.message$.next({text: 'Deleting', type: MessageType.Spinner  });

    this.service.deleteProject(this.project$.value)
    .then
    (
      succeeded =>
      {
        this.close();
      },
      rejected => this.handleError(rejected.message)
    );
  }

  public save(): void {

    this.disableInput$.next(true);
    this.message$.next({text: 'Saving', type: MessageType.Spinner  });

    this.service.saveProject(this.project$.value)
    .then
    (
      succeeded =>
      {
        this.message$.next({text: 'Saving complete', type: MessageType.Info  });
        this.handle(succeeded, this.project$);
      },
      rejected => this.handleError(rejected.message)
    );
  }

  private handleError(error: string): void
  {
    this.disableInput$.next(false);
    this.message$.next({text: error, type: MessageType.Error  });
    console.log(error);
  }


  public uploadFile (files : File[]) {
    if (files.length === 0) {
      return;
    }
 
    this.service.uploadFile(files[0])
    .then
    (
      ok => {
        console.log('Upload success.');
        this.uploadFinished(ok.data);
      },
      notok => this.handleError(notok)
    );
  }

  public uploadFinished = (event) => {
    console.log(event);
  }

}
