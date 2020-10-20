import { BehaviorSubject } from 'rxjs';
import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { PublicService } from 'src/app/core/public.service';
import { Introduction } from 'src/app/introduction/introduction.model';
import { MessageDescription, MessageType } from 'src/app/shared/message/message.component';
import { RequestResult, Incident } from 'src/app/shared/request-result.model';
import { ResourcesService } from 'src/app/core/resources.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-introduction',
  templateUrl: './introduction.component.html',
  styleUrls: ['./introduction.component.scss'],
})
export class IntroductionComponent implements OnInit {
  public introduction$: BehaviorSubject<Introduction> = new BehaviorSubject<Introduction>(null);
  public message$: BehaviorSubject<MessageDescription> = new BehaviorSubject<MessageDescription>({ type: MessageType.Spinner });

  public constructor(
    private service: PublicService,
    public textMessages: ResourcesService,
    titleService: Title
  ) {
    titleService.setTitle(
      this.textMessages.TitlePageIntroduction + environment.siteName
    );
  }

  public ngOnInit(): void {
    this.service.getIntroduction().then(
      (win) => this.handleIntroduction(win),
      (fail) => this.handleError(fail)
    );
  }

  private handleIntroduction(result: RequestResult<Introduction>): void {
    if (result.isSucceed) {
      this.introduction$.next(result.data);
      this.message$.next(null);
    } else {
      this.handleIncident(result.error);
    }
  }

  private handleIncident(error: Incident): void {
    console.log(error);
    this.message$.next({ text: error.message, type: MessageType.Error });
  }

  private handleError(error: any): void {
    console.log(error);

    if (error.name !== undefined) {
      this.message$.next({ text: error.name, type: MessageType.Error });
    } else {
      this.message$.next({ text: error, type: MessageType.Error });
    }
  }
}
