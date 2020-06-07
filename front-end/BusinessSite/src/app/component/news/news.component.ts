import { Component, Input } from '@angular/core';

import { News } from 'src/app/model/News';

@Component({
  selector: 'app-news',
  templateUrl: './news.component.html',
  styleUrls: ['./news.component.scss'],
})

export class NewsComponent {
  @Input()
  public news: News;
}
