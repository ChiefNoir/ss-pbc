import { Component, AfterViewInit, ViewChild } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

import { DataService } from 'src/app/service/data.service';
import { RequestResult } from 'src/app/model/RequestResult';
import { Introduction } from 'src/app/model/Introduction';
import { MessageDescription, MessageType } from 'src/app/component/message/message.component';
import { ExternalUrl } from 'src/app/model/ExternalUrl';
import { MatTable } from '@angular/material/table';

@Component({
  selector: 'app-admin-edit-introduction',
  templateUrl: './admin-edit-introduction.component.html',
  styleUrls: ['./admin-edit-introduction.component.scss'],
})

export class AdminEditIntroductionComponent implements AfterViewInit {
  private service: DataService;
  public columnsInner: string[] = [ 'name', 'url', 'btn'];
  @ViewChild('externalUrlsTable') externalUrlsTable: MatTable<any>;

  public introduction$: BehaviorSubject<Introduction> = new BehaviorSubject<Introduction>(null);
  public message$: BehaviorSubject<MessageDescription> = new BehaviorSubject<MessageDescription>({text: 'Loading', type: MessageType.Spinner  });
  public isDisabled: boolean = false;

  public constructor(service: DataService) {
    this.service = service;
  }

  ngAfterViewInit() {
    this.introduction$.next(null);

    this.service.getIntroduction()
    .then
    (
      (result) => this.handle(result, {text: 'Load complete', type: MessageType.Info }),
      (error) => this.handleError(error)
    );
  }

  public add(): void
  {
    if(this.introduction$.value.externalUrls) {
      this.introduction$.value.externalUrls.push(new ExternalUrl());
  
      this.externalUrlsTable.renderRows();
      }
      else {
        this.introduction$.value.externalUrls = new Array<ExternalUrl>();
        this.introduction$.value.externalUrls.push(new ExternalUrl());
      }
  }

  public save(): void
  {
    this.message$.next({text: 'Saving', type: MessageType.Spinner });
    this.isDisabled = true;

    if(this.introduction$.value.posterUrl !== this.imageSrc)
    {
      this.service.uploadFile(this.file)
    .then
    (
      ok => {
        this.introduction$.value.posterUrl = ok.data;
        this.service.updateIntroduction(this.introduction$.value).then
        (
          kk => {
            this.handle(kk, {text: 'Saving complete', type: MessageType.Info });
          },
          notok2 => this.handleError(notok2)
        );
      },
      notok => this.handleError(notok)
    );
    }
    else
    {
      this.service.updateIntroduction(this.introduction$.value).then
        (
          kk => {
            this.handle(kk, {text: 'Saving complete', type: MessageType.Info });
          },
          notok2 => this.handleError(notok2)
        );
    }
  }

  public refresh(): void
  {
    this.introduction$.next(null);

    this.service.getIntroduction()
    .then
    (
      (result) => this.handle(result, {text: 'Load complete', type: MessageType.Info }),
      (error) => this.handleError(error)
    );
  }

  public imageSrc: string;
  private file: File;
  public uploadFile(files: File[]) {
    if (files.length === 0) {
      return;
    }
    
    if (files && files[0]) {
      const file = files[0];

      const reader = new FileReader();
      reader.onload = e => this.imageSrc = reader.result as string;

      this.file = files[0]; 
      reader.readAsDataURL(file);
  }
    }

    public removeUrl(extUrl: ExternalUrl ): void {
      const index = this.introduction$.value.externalUrls.indexOf(extUrl, 0);
  if (index > -1) {
    this.introduction$.value.externalUrls.splice(index, 1);
  }
  this.externalUrlsTable.renderRows();
    }


  public deleteFile()
  {
    this.imageSrc = '';
    this.introduction$.value.posterUrl = '';
    // this.filename = 'no';
  }

  private handle(result: RequestResult<Introduction>, description: MessageDescription): void {
    if (result.isSucceed) {
      this.introduction$.next(result.data);
      this.imageSrc = result.data.posterUrl;
      this.message$.next(description);
     this.isDisabled = false;
    } else{
      this.handleError(result.errorMessage);
      this.isDisabled = false;
    }
  }

  private handleError(error: any): void {
    this.message$.next({text: error, type: MessageType.Error });
    console.log(error);
  }

}
