import { Component, AfterViewInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { BehaviorSubject } from 'rxjs';

import { environment } from 'src/environments/environment';
import { DataService } from 'src/app/service/data.service';
import { RequestResult } from 'src/app/model/RequestResult';
import { Introduction } from 'src/app/model/Introduction';

@Component({
  selector: 'app-introduction',
  templateUrl: './introduction.component.html',
  styleUrls: ['./introduction.component.scss'],
})

export class IntroductionComponent implements AfterViewInit {
  private service: DataService;

  public introduction$: BehaviorSubject<Introduction> = new BehaviorSubject<Introduction>(null);

  public constructor(service: DataService, titleService: Title) {
    this.service = service;

    titleService.setTitle(environment.siteName);
  }

  ngAfterViewInit(): void {
    this.service.getIntroduction()
                .then
                (
                  (result) => { this.handle(result, this.introduction$); }
                );
  }

  private handle<T>(result: RequestResult<T>, content: BehaviorSubject<T>): void {
    if (result.isSucceed) {
    content.next(result.data);
    } else{
      this.handleError(result.errorMessage);
    }
  }

  private handleError(error: any): void {
    // TODO: react properly
    console.log(error);
  }
}
