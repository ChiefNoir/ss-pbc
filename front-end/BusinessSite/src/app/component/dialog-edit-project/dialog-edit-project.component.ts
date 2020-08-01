import { Component, Input, AfterContentInit, OnInit, ViewChild, Inject } from '@angular/core';
import { Project } from 'src/app/model/Project';
import { BehaviorSubject } from 'rxjs';
import { DataService } from 'src/app/service/data.service';
import { RequestResult } from 'src/app/model/RequestResult';
import { Category } from 'src/app/model/Category';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { ExternalUrl } from 'src/app/model/ExternalUrl';
import { MatTable } from '@angular/material/table';
import { MessageType, MessageDescription } from '../message/message.component';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { StaticNames } from 'src/app/common/StaticNames';

@Component({
  selector: 'app-dialog-edit-project.',
  templateUrl: './dialog-edit-project..component.html',
  styleUrls: ['./dialog-edit-project..component.scss']
})

export class DialogEditProjectComponent implements OnInit
{
  public columnsInner: string[] = [ 'name', 'url', 'btn'];
  @ViewChild('externalUrlsTable') externalUrlsTable: MatTable<any>;
  private dialog: MatDialogRef<DialogEditProjectComponent>;

  private service: DataService;

  @Input()
  public code: string;

  public project$: BehaviorSubject<Project> = new BehaviorSubject<Project>(null);
  public categories$: BehaviorSubject<Category[]> = new BehaviorSubject<Category[]>(null);



  public disableInput$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  public message$: BehaviorSubject<MessageDescription> = new BehaviorSubject<MessageDescription>(null);

  public loadingMessage: MessageDescription = {text: 'Loading', type: MessageType.Spinner };

  constructor(service: DataService, dialog: MatDialogRef<DialogEditProjectComponent>, @Inject(MAT_DIALOG_DATA) projectCode: string)
  {
    this.service = service;
    this.dialog = dialog;
    this.code = projectCode;
  }

  public ngOnInit()
  {
    this.refresh();
  }

  public refresh(): void
  {
    this.disableInput$.next(true);
    this.message$.next({text: StaticNames.LoadInProgress, type: MessageType.Spinner  });

    this.service.getCategories().then
    (
      categorySucceeded =>
      {
        this.categories$.next(categorySucceeded.data.filter(category => category.isEverything === false));

        if (this.code)
        {
          this.service.getProject(this.code)
                      .then
                      (
                        succeeded => this.handleProject(succeeded),
                        rejected => this.handleError(rejected.message)
                      );
        }
        else
        {
          const prj = new Project();
          prj.category = categorySucceeded.data.filter(category => category.isEverything === false)[0];
          this.project$.next(prj);
          this.message$.next({text: StaticNames.LoadComplete, type: MessageType.Info });
        }
      },
      categoryRejected => this.handleError(categoryRejected.message)
    );
  }

  public addUrl(): void
  {
    if (this.project$.value.externalUrls)
    {
      this.project$.value.externalUrls.push(new ExternalUrl());
      this.externalUrlsTable.renderRows();
    }
    else
    {
      this.project$.value.externalUrls = new Array<ExternalUrl>();
      this.project$.value.externalUrls.push(new ExternalUrl());
    }
  }

  public removeUrl(extUrl: ExternalUrl ): void
  {
    const index = this.project$.value.externalUrls.indexOf(extUrl, 0);

    if (index > -1)
    {
      this.project$.value.externalUrls.splice(index, 1);
      this.externalUrlsTable.renderRows();
    }
  }

  public save(): void
  {
    this.disableInput$.next(true);
    this.message$.next({text: 'Saving', type: MessageType.Spinner  });

    this.service.saveProject(this.project$.value)
                .then
                (
                  succeeded =>
                  {
                    this.message$.next({text: StaticNames.SaveComplete, type: MessageType.Info  });
                    this.handleProject(succeeded);
                  },
                  rejected => this.handleError(rejected.message)
                );
  }





  public close(): void
  {
    this.dialog.close();
  }



  public delete(): void
  {
    this.disableInput$.next(true);
    this.message$.next({text: 'Deleting', type: MessageType.Spinner  });

    this.service.deleteProject(this.project$.value)
                .then
                (
                  succeeded =>
                  {
                    this.message$.next({text: StaticNames.DeleteComplete, type: MessageType.Info });
                  },
                  rejected => this.handleError(rejected.message)
    );
  }



  private handleError(error: string): void
  {
    this.disableInput$.next(false);
    this.message$.next({text: error, type: MessageType.Error  });
  }


  public uploadFile (files : File[]) {
    if (files.length === 0) {
      return;
    }

    this.service.uploadFile(files[0])
    .then
    (
      ok => {
        this.project$.value.posterUrl = ok.data;

      },
      notok => this.handleError(notok)
    );
  }

  public deleteFile()
  {
    this.project$.value.posterUrl = '';
    // this.filename = 'no';
  }

  private handleProject(result: RequestResult<Project>): void
  {
    if (result.isSucceed)
    {
      this.disableInput$.next(false);
      this.project$.next(result.data);
      this.message$.next({text: StaticNames.LoadComplete, type: MessageType.Info });
    }
    else
    {
      this.handleError(result.errorMessage);
    }
  }
}
