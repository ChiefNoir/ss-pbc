import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-paginator',
  templateUrl: './paginator.component.html',
  styleUrls: ['./paginator.component.scss'],
})

export class PaginatorComponent {
  @Input()
  public navigateCallback: (value: number) => void;

  @Input()
  public maxPage: number;

  @Input()
  public minPage: number;

  @Input()
  public currentPage: number;

  @Input()
  public nextPageUrl: string;

  @Input()
  public previousPageUrl: string;
}
