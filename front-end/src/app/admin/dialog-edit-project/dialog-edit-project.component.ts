import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { MatTable } from '@angular/material/table';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

import { PublicService } from '../../shared/services/public.service';
import { ResourcesService } from '../../shared/services/resources.service';

import { Project } from '../../shared/models/project.model';
import { RequestResult } from '../../shared/models/request-result.interface';
import { Incident } from '../../shared/models/incident.interface';
import { Category } from '../../shared/models/category.model';
import { ExternalUrl } from '../../shared/models/external-url.model';
import { GalleryImage } from '../../shared/models/gallery-image.model';
import { MessageDescription, MessageType } from '../../shared/components/message/message.component';

import { PrivateService } from '../private.service';

@Component({
  selector: 'app-dialog-edit-project.',
  templateUrl: './dialog-edit-project.component.html',
  styleUrls: ['./dialog-edit-project.component.scss'],
})
export class DialogEditProjectComponent implements OnInit {
  @ViewChild('externalUrlsTable') externalUrlsTable: MatTable<any>;
  @ViewChild('galleryImagesTable') galleryImagesTable: MatTable<any>;

  public columnsInner: string[] = ['name', 'url', 'btn'];
  public columnsGallery: string[] = ['imageUrl', 'extraUrl', 'btn'];

  public categories$: BehaviorSubject<Category[]> = new BehaviorSubject<Category[]>(null);
  public disableInput$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  public message$: BehaviorSubject<MessageDescription> = new BehaviorSubject<MessageDescription>(null);
  public project$: BehaviorSubject<Project> = new BehaviorSubject<Project>(null);

  private code: string;

  constructor(
    private service: PrivateService,
    public textMessages: ResourcesService,
    private publicService: PublicService,
    private dialog: MatDialogRef<DialogEditProjectComponent>,
    @Inject(MAT_DIALOG_DATA) projectCode: string
  ) {
    this.code = projectCode;
  }

  public ngOnInit(): void {
    this.refresh();
  }

  public refresh(): void {
    this.disableInput$.next(true);
    this.message$.next({ type: MessageType.Spinner });

    this.publicService
        .getCategories()
        .subscribe
        (
          categoryWin =>
          {
            this.categories$.next
            (
              categoryWin.data.filter(x => x.isEverything === false)
            );

            if (this.code) {
              this.publicService
                  .getProject(this.code)
                  .subscribe(
                    win =>
                      this.handleProject(win, {text: this.textMessages.LoadComplete, type: MessageType.Info}),
                    fail => this.handleError(fail));
            } else {
              const prj = new Project();
              prj.category = categoryWin.data.filter(x => x.isEverything === false)[0];
              this.project$.next(prj);

              this.message$.next({text: this.textMessages.InitializationComplete, type: MessageType.Info});
              this.disableInput$.next(false); }
          },
          categoryFail => this.handleError(categoryFail)
    );
  }

  public addUrl(): void {
    if (this.project$.value.externalUrls) {
      this.project$.value.externalUrls.push(new ExternalUrl());
      this.externalUrlsTable.renderRows();
    } else {
      this.project$.value.externalUrls = new Array<ExternalUrl>();
      this.project$.value.externalUrls.push(new ExternalUrl());
    }
  }

  public removeUrl(extUrl: ExternalUrl): void {
    const index = this.project$.value.externalUrls.indexOf(extUrl, 0);

    if (index > -1) {
      this.project$.value.externalUrls.splice(index, 1);
      this.externalUrlsTable.renderRows();
    }
  }

  public save(): void {
    this.disableInput$.next(true);
    this.message$.next({text: this.textMessages.SaveInProgress, type: MessageType.Spinner});

    this.service
        .saveProject(this.project$.value)
        .subscribe(
          win => {
            this.handleProject(win, {text: this.textMessages.SaveComplete, type: MessageType.Info}); },
          fail => this.handleError(fail));
  }

  public delete(): void {
    this.disableInput$.next(true);
    this.message$.next({text: this.textMessages.DeleteInProgress, type: MessageType.Info});

    this.service
        .deleteProject(this.project$.value)
        .subscribe(
          win => this.handleDelete(win),
          fail => this.handleError(fail));
  }

  public close(): void {
    this.dialog.close();
  }

  public selectFile(files: File[]): void {
    if (files.length === 0) {
      return;
    }

    if (files && files[0]) {
      const file = files[0];

      const reader = new FileReader();
      reader.onload = (e) => this.project$.value.posterPreview = reader.result as string;

      this.project$.value.posterToUpload = files[0];
      reader.readAsDataURL(file);
    }
  }

  public selectGalleryFile(files: File[], galleryImage: GalleryImage): void {
    if (files.length === 0) {
      return;
    }

    if (files && files[0]) {
      const file = files[0];

      const reader = new FileReader();
      reader.onload = (e) => galleryImage.localPreview = reader.result as string;

      galleryImage.readyToUpload = files[0];
      reader.readAsDataURL(file);
    }
  }

  public deleteFile(): void {
    this.project$.value.posterUrl = '';
    this.project$.value.posterPreview = '';
    this.project$.value.posterToUpload = null;
  }

  private handleDelete(result: RequestResult<boolean>): void {
    if (result.isSucceed) {
      this.project$.next(null);
      this.message$.next({text: this.textMessages.DeleteComplete, type: MessageType.Info});
    } else {
      this.handleError(result.error);
    }
  }

  private handleProject(result: RequestResult<Project>, msg: MessageDescription): void {
    if (result.isSucceed) {
      this.disableInput$.next(false);
      this.project$.next(result.data);
      this.message$.next(msg);
    } else {
      this.handleIncident(result.error);
    }
  }

  public addGalleryImage(): void {
    // hack for the times, when there is only one change: image
    const newItem = new GalleryImage();
    newItem.version = 0;

    if (this.project$.value.galleryImages) {
      this.project$.value.galleryImages.push(newItem);
      this.galleryImagesTable.renderRows();
    } else {
      this.project$.value.galleryImages = new Array<GalleryImage>();
      this.project$.value.galleryImages.push(newItem);
    }
  }

  public removeGalleryImage(item: GalleryImage): void {
    const index = this.project$.value.galleryImages.indexOf(item, 0);

    if (index > -1) {
      this.project$.value.galleryImages.splice(index, 1);
      this.galleryImagesTable.renderRows();
    }
  }

  private handleIncident(error: Incident): void {
    this.disableInput$.next(false);

    this.message$.next({ text: error.message, type: MessageType.Error });
  }

  private handleError(error: any): void {
    this.disableInput$.next(false);

    if (error.name !== undefined) {
      this.message$.next({ text: error.name, type: MessageType.Error });
    } else {
      this.message$.next({ text: error, type: MessageType.Error });
    }
  }
}
