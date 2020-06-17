import { Component, Input, AfterViewInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { DataService } from 'src/app/service/data.service';
import { RequestResult } from 'src/app/model/RequestResult';
import { Category } from 'src/app/model/Category';

@Component({
  selector: 'app-dialog-editor-category.component',
  templateUrl: './dialog-editor-category.component.html',
  styleUrls: ['./dialog-editor-category.component.scss']
})

export class DialogEditorCategoryComponent implements AfterViewInit
{
  private service: DataService;
  public category$: BehaviorSubject<Category> = new BehaviorSubject<Category>(null);

  @Input()
  public code: string;

  constructor(service: DataService){
    this.service = service;
  }

  ngAfterViewInit(): void {
    this.service.getCategory(this.code).then
    (
      (x) => this.handle(x, this.category$)
    );
  }

  private handle<T>(result: RequestResult<T>, content: BehaviorSubject<T>): void {
    if (result.isSucceed){

    content.next(result.data);
    } else
    {
      this.handleError(result.errorMessage);
    }
  }

  private handleError(error: any): void {
    console.log(error);
  }
}
