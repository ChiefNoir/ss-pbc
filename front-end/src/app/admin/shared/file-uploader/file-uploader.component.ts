import { Component, Output, EventEmitter, Input } from '@angular/core';
import { ResourcesService } from '../../../core/services/resources.service';

@Component({
  selector: 'app-file-uploader',
  templateUrl: './file-uploader.component.html',
  styleUrls: ['./file-uploader.component.scss'],
})
export class FileUploaderComponent {
  @Output()
  public uploadFiles: EventEmitter<File[]> = new EventEmitter<File[]>();

  @Output()
  public deleteFile: EventEmitter<any> = new EventEmitter();

  @Input()
  public filename: string;

  @Input()
  public disabled: boolean;

  constructor(public textMessages: ResourcesService) {}

  public upload(files: File[]): void {
    this.uploadFiles.emit(files);
  }

  public delete(): void {
    this.deleteFile.emit();
  }
}
