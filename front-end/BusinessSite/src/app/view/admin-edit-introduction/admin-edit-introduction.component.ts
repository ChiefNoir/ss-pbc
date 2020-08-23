import { Component, ViewChild, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

import { DataService } from 'src/app/service/data.service';
import { RequestResult, Incident } from 'src/app/model/RequestResult';
import { Introduction } from 'src/app/model/Introduction';
import { MessageDescription, MessageType } from 'src/app/component/message/message.component';
import { ExternalUrl } from 'src/app/model/ExternalUrl';
import { MatTable } from '@angular/material/table';

@Component({
  selector: 'app-admin-edit-introduction',
  templateUrl: './admin-edit-introduction.component.html',
  styleUrls: ['./admin-edit-introduction.component.scss'],
})

export class AdminEditIntroductionComponent implements OnInit
{
  private service: DataService;
  private file: File;

  public columnsInner: string[] = [ 'name', 'url', 'btn'];
  @ViewChild('externalUrlsTable') externalUrlsTable: MatTable<any>;

  public introduction$: BehaviorSubject<Introduction> = new BehaviorSubject<Introduction>(null);
  public message$: BehaviorSubject<MessageDescription> = new BehaviorSubject<MessageDescription>({text: 'Loading', type: MessageType.Spinner  });
  public isDisabled: boolean = false;
  public imageSrc: string;

  public constructor(service: DataService)
  {
    this.service = service;
  }

  public ngOnInit()
  {
    this.introduction$.next(null);

    this.service.getIntroduction()
                .then
                (
                  result => this.handle(result, {text: 'Load complete', type: MessageType.Info }),
                  error => this.handleError(error)
                );
  }

  public addExternalUrl(): void
  {
    if (this.introduction$.value.externalUrls)
    {
      this.introduction$.value.externalUrls.push(new ExternalUrl());
      this.externalUrlsTable.renderRows();
    }
    else
    {
      this.introduction$.value.externalUrls = new Array<ExternalUrl>();
      this.introduction$.value.externalUrls.push(new ExternalUrl());
    }
  }

  public save(): void
  {
    this.message$.next({text: 'Saving', type: MessageType.Spinner });
    this.isDisabled = true;

    if (this.introduction$.value.posterUrl !== this.imageSrc)
    {
      this.service
          .uploadFile(this.file)
          .then
          (
            resultImage =>
            {
              if (resultImage.isSucceed)
              {
                this.introduction$.value.posterUrl = resultImage.data;
                this.service.updateIntroduction(this.introduction$.value)
                            .then(
                              resultIntroduction => this.handle(resultIntroduction, {text: 'Saving complete', type: MessageType.Info }),
                              rejectIntroduction => this.handleError(rejectIntroduction)
                                );
              }
              else
              {
                this.handleIncident(resultImage.error);
                return;
              }
            },
            rejectImage => this.handleError(rejectImage)
          );
    }
    else
    {
      this.service.updateIntroduction(this.introduction$.value).then
        (
          result => this.handle(result, {text: 'Saving complete', type: MessageType.Info }),
          reject => this.handleError(reject)
        );
    }
  }

  public refresh(): void
  {
    this.introduction$.next(null);

    this.service.getIntroduction()
        .then
        (
          result => this.handle(result, {text: 'Load complete', type: MessageType.Info }),
          reject => this.handleError(reject)
        );
  }

  public uploadFile(files: File[])
  {
    if (!files || files.length === 0 || !files[0]) { return; }

    const file = files[0];
    const reader = new FileReader();

    reader.onload = e => this.imageSrc = reader.result as string;

    this.file = files[0];
    reader.readAsDataURL(file);
  }

  public deleteFile()
  {
    this.imageSrc = '';
    this.introduction$.value.posterUrl = '';
  }

  public removeUrl(extUrl: ExternalUrl ): void
  {
    const index = this.introduction$.value.externalUrls.indexOf(extUrl, 0);
    if (index <= -1) { return; }

    this.introduction$.value.externalUrls.splice(index, 1);
    this.externalUrlsTable.renderRows();
  }

  private handle(result: RequestResult<Introduction>, description: MessageDescription): void
  {
    if (result.isSucceed)
    {
      this.introduction$.next(result.data);
      this.imageSrc = result.data.posterUrl;
      this.message$.next(description);
      this.isDisabled = false;
    }
    else
    {
      this.handleIncident(result.error);
      this.isDisabled = false;
    }
  }

  private handleIncident(error: Incident): void
  {
    this.message$.next({text: error.code + ' : ' + error.message + '<br/>' + error.detail + '<br/>' , type: MessageType.Error });
  }

  private handleError(error: any): void
  {
    console.log(error);

    if (error.name !== undefined)
    {
      this.message$.next({text: error.name, type: MessageType.Error });
    }
    else
    {
      this.message$.next({text: error, type: MessageType.Error });
    }
  }
}
