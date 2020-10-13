import { Component, Output, EventEmitter, Input } from '@angular/core';
import { TextMessages } from 'src/app/shared/text-messages.resources';

@Component({
  selector: 'app-file-uploader',
  templateUrl: './file-uploader.component.html',
  styleUrls: ['./file-uploader.component.scss']
})

export class FileUploaderComponent
{
  @Output()
  public uploadFiles = new EventEmitter<File[]>();

  @Output()
  public deleteFile = new EventEmitter();

  @Input()
  public filename: string;

  @Input()
  public disabled: boolean;

  public textMessages: TextMessages = new TextMessages();

  public upload(files: File[]): void
  {
    this.uploadFiles.emit(files);
  }

  public delete(): void
  {
    this.deleteFile.emit();
  }
}
