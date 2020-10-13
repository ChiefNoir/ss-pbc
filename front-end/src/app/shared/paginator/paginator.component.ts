import { Component, Input, Output, EventEmitter } from '@angular/core';
import { TextMessages } from 'src/app/shared/text-messages.resources';

@Component({
  selector: 'app-paginator',
  templateUrl: './paginator.component.html',
  styleUrls: ['./paginator.component.scss'],
})

export class PaginatorComponent
{
  @Input()
  public maxPage: number;

  @Input()
  public minPage: number;

  @Input()
  public currentPage: number;


  @Output()
  public changePage: EventEmitter<number> = new EventEmitter<number>();

  @Output()
  public nextPageClick = new EventEmitter();

  @Output()
  public previousPageClick = new EventEmitter();

  public textMessages: TextMessages = new TextMessages();

  public changePageEnterPress(pageNumber: number)
  {
    this.changePage.emit(pageNumber);
  }

  public btnNextClick()
  {
    this.nextPageClick.emit();
  }

  public btnPreviousClick()
  {
    this.previousPageClick.emit();
  }
}
