import { AfterViewChecked, ChangeDetectorRef, Component } from '@angular/core';
import { Router } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { environment } from 'src/environments/environment';
import { AuthGuard } from './guards/auth.guard';
import { TextMessages } from './resources/text-messages.resources';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})

export class AppComponent implements AfterViewChecked
{
  public authGuard: AuthGuard;
  public router: Router;
  public textMessages: TextMessages = new TextMessages();

  private changeDetectorRef: ChangeDetectorRef;
  private isValidating: boolean = false;

  public constructor(titleService: Title, authGuard: AuthGuard, router: Router, changeDetectorRef: ChangeDetectorRef)
  {
    this.authGuard = authGuard;
    this.changeDetectorRef = changeDetectorRef;
    this.router = router;

    titleService.setTitle(environment.siteName);
  }

  public ngAfterViewChecked(): void
  {
    // In dev mode change detection adds an additional turn
    // after every regular change detection run to check if the model has changed
    const isValidating = this.authGuard.validating$.value;
    if (isValidating !== this.isValidating)
    { // check if it change, tell CD update view
      this.isValidating = isValidating;
      this.changeDetectorRef.detectChanges();
    }
  }

}
