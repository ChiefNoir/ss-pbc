import { Component, OnInit } from '@angular/core';
import { BehaviorSubject, from } from 'rxjs';

import { DataService } from 'src/app/service/data.service';
import { RequestResult } from 'src/app/model/RequestResult';
import { News } from 'src/app/model/News';


@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.scss']
  })

export class HomeComponent implements OnInit {

  private service: DataService;
  public news$: BehaviorSubject<Array<News>> = new BehaviorSubject<Array<News>>(null);

  public constructor(service: DataService) {
    this.service = service;
  }


  public ngOnInit(): void {
    this.service.getNews()
                .subscribe
                (
                  result => this.handleRequestResult(result),
                  error => this.handleError(error)
                );
  }

  private handleRequestResult(result: RequestResult<Array<News>>): void {

    if (result.isSucceed)
    {
      this.news$.next(result.data);
    }
    else
    {
      this.handleError(result.errorMessage);
    }
  }

  private handleError(error: any): void {

    // TODO: react properly
    console.log(error);
  }
}
