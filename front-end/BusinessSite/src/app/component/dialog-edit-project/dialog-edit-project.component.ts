import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import { Project } from 'src/app/model/Project';
import { BehaviorSubject } from 'rxjs';
import { DataService } from 'src/app/service/data.service';
import { RequestResult, Incident } from 'src/app/model/RequestResult';
import { Category } from 'src/app/model/Category';
import { ExternalUrl } from 'src/app/model/ExternalUrl';
import { MatTable } from '@angular/material/table';
import { MessageType, MessageDescription } from '../message/message.component';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { StaticNames } from 'src/app/common/StaticNames';
import { GalleryImage } from 'src/app/model/GalleryImage';

@Component({
  selector: 'app-dialog-edit-project.',
  templateUrl: './dialog-edit-project.component.html',
  styleUrls: ['./dialog-edit-project.component.scss']
})

export class DialogEditProjectComponent implements OnInit
{
  public columnsInner: string[] = [ 'name', 'url', 'btn'];
  public columnsGallery: string[] = [ 'imageUrl', 'extraUrl', 'btn'];

  private code: string;
  private service: DataService;
  public categories$: BehaviorSubject<Category[]> = new BehaviorSubject<Category[]>(null);
  public disableInput$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  public loadingMessage: MessageDescription = {text: 'Loading', type: MessageType.Spinner };
  public message$: BehaviorSubject<MessageDescription> = new BehaviorSubject<MessageDescription>(null);
  public project$: BehaviorSubject<Project> = new BehaviorSubject<Project>(null);
  public title$: BehaviorSubject<string> = new BehaviorSubject<string>('Project properties');

  @ViewChild('externalUrlsTable') externalUrlsTable: MatTable<any>;
  @ViewChild('galleryImagesTable') galleryImagesTable: MatTable<any>;
  private dialog: MatDialogRef<DialogEditProjectComponent>;

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
                        succeeded => this.handleProject(succeeded, {text: StaticNames.LoadComplete, type: MessageType.Info }),
                        rejected => this.handleError(rejected)
                      );
        }
        else
        {
          const prj = new Project();
          prj.category = categorySucceeded.data.filter(category => category.isEverything === false)[0];
          this.project$.next(prj);
          this.message$.next({text: StaticNames.InitializationComplete, type: MessageType.Info });
          this.disableInput$.next(false);
        }
      },
      categoryRejected => this.handleError(categoryRejected)
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

    this.message$.next({text: StaticNames.SaveInProgress, type: MessageType.Spinner  });

    this.service.saveProject(this.project$.value).then
    (
      kk =>
      {
        this.handleProject(kk, {text: StaticNames.SaveComplete, type: MessageType.Info });
      },
      notok2 => this.handleError(notok2)
    );
  }

  public delete(): void
  {
    this.disableInput$.next(true);
    this.message$.next({text: StaticNames.DeleteInProgress, type: MessageType.Info });

    this.service.deleteProject(this.project$.value)
                .then
                (
                  succeeded =>
                  {
                    this.project$.next(null);
                    this.message$.next({text: StaticNames.DeleteComplete, type: MessageType.Info });
                  },
                  rejected => this.handleError(rejected)
    );
  }

  public close(): void
  {
    this.dialog.close();
  }

  public selectFile(files: File[])
  {
    if (files.length === 0) { return; }

    if (files && files[0])
    {
      const file = files[0];

      const reader = new FileReader();
      reader.onload = e => this.project$.value.posterPreview = reader.result as string;

      this.project$.value.posterToUpload = files[0];
      reader.readAsDataURL(file);
    }
  }

  public selectGalleryFile(files: File[], galleryImage: GalleryImage)
  {
    if (files.length === 0) { return; }

    if (files && files[0])
    {
      const file = files[0];

      const reader = new FileReader();
      reader.onload = e => galleryImage.localPreview = reader.result as string;

      galleryImage.readyToUpload = files[0];
      reader.readAsDataURL(file);
    }
  }

  public deleteFile()
  {
    this.project$.value.posterUrl = '';
    this.project$.value.posterPreview = '';
    this.project$.value.posterToUpload = null;
  }

  private handleProject(result: RequestResult<Project>, msg: MessageDescription): void
  {
    if (result.isSucceed)
    {
      this.disableInput$.next(false);
      this.project$.next(result.data);
      this.message$.next(msg);
    }
    else
    {
      this.handleIncident(result.error);
    }
  }



  public addGalleryImage(): void
  {
    // hack for the times, when there is only img
    const newItem = new GalleryImage();
    newItem.version = 0;

    if (this.project$.value.galleryImages)
    {
      this.project$.value.galleryImages.push(newItem);
      this.galleryImagesTable.renderRows();
    }
    else
    {
      this.project$.value.galleryImages = new Array<GalleryImage>();
      this.project$.value.galleryImages.push(newItem);
    }
  }

  public removeGalleryImage(item: GalleryImage): void
  {
    const index = this.project$.value.galleryImages.indexOf(item, 0);

    if (index > -1)
    {
      this.project$.value.galleryImages.splice(index, 1);
      this.galleryImagesTable.renderRows();
    }
  }

  private handleIncident(error: Incident): void
  {
    this.disableInput$.next(false);
    this.message$.next({text: error.code + ' : ' + error.message + '<br/>' + error.detail + '<br/>' , type: MessageType.Error });
  }

  private handleError(error: any): void
  {
    this.disableInput$.next(false);
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
