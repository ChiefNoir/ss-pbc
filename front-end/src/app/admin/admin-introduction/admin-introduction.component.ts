import { Component, ViewChild, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

import { PublicService } from '../../core/services/public.service';
import { RequestResult } from '../../shared/request-result.interface';
import { Incident } from '../../shared/incident.interface'
import { Introduction } from '../../introduction/introduction.model';
import { MessageDescription, MessageType } from '../../shared/message/message.component';
import { ExternalUrl } from '../../shared/external-url.model';
import { MatTable } from '@angular/material/table';
import { ResourcesService } from '../../core/services/resources.service';
import { PrivateService } from '../private.service';
import { Title } from '@angular/platform-browser';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-admin-introduction',
  templateUrl: './admin-introduction.component.html',
  styleUrls: ['./admin-introduction.component.scss'],
})
export class AdminIntroductionComponent implements OnInit {
  @ViewChild('externalUrlsTable') externalUrlsTable: MatTable<any>;

  public introduction$: BehaviorSubject<Introduction> = new BehaviorSubject<Introduction>(null);
  public message$: BehaviorSubject<MessageDescription> = new BehaviorSubject<MessageDescription>({ text: 'Loading', type: MessageType.Spinner });
  public columnsInner: string[] = ['name', 'url', 'btn'];
  public isDisabled: boolean = false;

  public constructor(
    private service: PrivateService,
    public textMessages: ResourcesService,
    private publicService: PublicService,
    titleService: Title
  ) {
    titleService.setTitle(environment.siteName);
  }

  public ngOnInit(): void {
    this.introduction$.next(null);
    
    this.publicService
        .getIntroduction()
        .subscribe(
          win => this.handle(win, { text: this.textMessages.LoadComplete, type: MessageType.Info }),
          fail => this.handleError(fail));
  }

  public addExternalUrl(): void {
    if (this.introduction$.value.externalUrls) {
      this.introduction$.value.externalUrls.push(new ExternalUrl());
      this.externalUrlsTable.renderRows();
    } else {
      this.introduction$.value.externalUrls = new Array<ExternalUrl>();
      this.introduction$.value.externalUrls.push(new ExternalUrl());
    }
  }

  public save(): void {
    this.message$.next({ text:  this.textMessages.SaveInProgress, type: MessageType.Spinner });
    this.isDisabled = true;

    this.service
        .saveIntroduction(this.introduction$.value)
        .subscribe(
          win => this.handle(win, {text: this.textMessages.SaveComplete, type: MessageType.Info}),
          fail => this.handleError(fail));
  }

  public refresh(): void {
    this.introduction$.next(null);

    this.publicService
        .getIntroduction()
        .subscribe(
          win => this.handle(win, { text: 'Load complete', type: MessageType.Info }),
          fail => this.handleError(fail));
  }

  public uploadFile(files: File[]): void {
    if (!files || files.length === 0 || !files[0]) {
      return;
    }

    const file = files[0];
    const reader = new FileReader();

    reader.onload = (e) =>
      (this.introduction$.value.posterPreview = reader.result as string);

    this.introduction$.value.posterToUpload = files[0];
    reader.readAsDataURL(file);
  }

  public deleteFile(): void {
    this.introduction$.value.posterPreview = '';
    this.introduction$.value.posterToUpload = null;
    this.introduction$.value.posterUrl = '';
  }

  public removeUrl(extUrl: ExternalUrl): void {
    const index = this.introduction$.value.externalUrls.indexOf(extUrl, 0);
    if (index <= -1) {
      return;
    }

    this.introduction$.value.externalUrls.splice(index, 1);
    this.externalUrlsTable.renderRows();
  }

  private handle(result: RequestResult<Introduction>, description: MessageDescription): void {
    if (result.isSucceed) {
      this.introduction$.next(result.data);
      this.message$.next(description);
      this.isDisabled = false;
    } else {
      this.handleIncident(result.error);
      this.isDisabled = false;
    }
  }

  private handleIncident(error: Incident): void {
    this.isDisabled = false;

    this.message$.next({ text: error.message, type: MessageType.Error });
  }

  private handleError(error: any): void {
    this.isDisabled = false;

    if (error.name !== undefined) {
      this.message$.next({ text: error.name, type: MessageType.Error });
    } else {
      this.message$.next({ text: error, type: MessageType.Error });
    }
  }
}
