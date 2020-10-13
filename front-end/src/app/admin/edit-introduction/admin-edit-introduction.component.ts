import { Component, ViewChild, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

import { DataService } from 'src/app/core/data.service';
import { RequestResult, Incident } from 'src/app/shared/request-result.model';
import { Introduction } from 'src/app/introduction/introduction.model';
import { MessageDescription, MessageType } from 'src/app/shared/message/message.component';
import { ExternalUrl } from 'src/app/shared/external-url.model';
import { MatTable } from '@angular/material/table';
import { AuthGuard } from 'src/app/core/auth.guard';
import { Router } from '@angular/router';
import { TextMessages } from 'src/app/shared/text-messages.resources';
import { PrivateService } from 'src/app/core/private.service';

@Component({
  selector: 'app-admin-edit-introduction',
  templateUrl: './admin-edit-introduction.component.html',
  styleUrls: ['./admin-edit-introduction.component.scss'],
})

export class AdminEditIntroductionComponent implements OnInit
{
  private service: PrivateService;
  private publicService: DataService;

  public columnsInner: string[] = [ 'name', 'url', 'btn'];
  @ViewChild('externalUrlsTable') externalUrlsTable: MatTable<any>;

  public introduction$: BehaviorSubject<Introduction> = new BehaviorSubject<Introduction>(null);
  public message$: BehaviorSubject<MessageDescription> = new BehaviorSubject<MessageDescription>({text: 'Loading', type: MessageType.Spinner  });
  public isDisabled: boolean = false;
  private authGuard: AuthGuard;
  private router: Router;
  public textMessages: TextMessages = new TextMessages();

  public constructor(service: PrivateService,  publicService: DataService, authGuard: AuthGuard, router: Router)
  {
    this.service = service;
    this.authGuard = authGuard;
    this.router = router;
    this.publicService = publicService;
  }

  public async ngOnInit(): Promise<void>
  {
    await this.authGuard.checkIsLogged();
    if (this.authGuard.isLoggedIn$.value)
    {
      this.introduction$.next(null);

      this.publicService.getIntroduction()
                  .then
                  (
                    result => this.handle(result, {text: 'Load complete', type: MessageType.Info }),
                    error => this.handleError(error)
                  );
    }
    else
    {
      this.router.navigate(['/login']);
    }
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

    this.service.saveIntroduction(this.introduction$.value).then
    (
      result => this.handle(result, {text: 'Saving complete', type: MessageType.Info }),
      reject => this.handleError(reject)
    );
  }

  public refresh(): void
  {
    this.introduction$.next(null);

    this.publicService.getIntroduction()
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

    reader.onload = e => this.introduction$.value.posterPreview = reader.result as string;

    this.introduction$.value.posterToUpload = files[0];
    reader.readAsDataURL(file);
  }

  public deleteFile()
  {
    this.introduction$.value.posterPreview = '';
    this.introduction$.value.posterToUpload = null;
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
    this.isDisabled = false;
    console.log(error);

    this.message$.next({text: error.message, type: MessageType.Error });
  }

  private handleError(error: any): void
  {
    this.isDisabled = false;
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
