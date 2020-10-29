import { AfterViewChecked, ChangeDetectorRef, Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { environment } from '../environments/environment';
import { AuthGuard } from './core/services/auth.guard';
import { ResourcesService } from './core/services/resources.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements AfterViewChecked {
  private isValidating: boolean = false;

  public constructor(
    titleService: Title,
    public authGuard: AuthGuard,
    public textMessages: ResourcesService,
    private changeDetectorRef: ChangeDetectorRef
  ) {
    titleService.setTitle(environment.siteName);
  }

  public ngAfterViewChecked(): void {
    // NOTE: right now this thing is useless, but, it will remain here just in case
    // In dev mode change detection adds an additional turn
    // after every regular change detection run to check if the model has changed
    const isValidating = this.authGuard.validating$.value;
    if (isValidating !== this.isValidating) {
      // check if it change, tell CD update view
      this.isValidating = isValidating;
      this.changeDetectorRef.detectChanges();
    }
  }
}
