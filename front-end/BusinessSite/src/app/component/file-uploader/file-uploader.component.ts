import { Component, Output, EventEmitter, Input } from '@angular/core';

@Component({
  selector: 'app-file-uploader',
  templateUrl: './file-uploader.component.html',
  styleUrls: ['./file-uploader.component.scss']
})

export class FileUploaderComponent {

  @Output()
  uploadFiles = new EventEmitter<File[]>();

  @Output()
  deleteFile = new EventEmitter();

  @Input()
  filename: string;

  public upload(files: File[])
  {
    this.uploadFiles.emit(files);
  }

  public delete(): void {
    this.deleteFile.emit();
  }
}
