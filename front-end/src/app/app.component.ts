import { AfterViewChecked, ChangeDetectorRef, Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { environment } from 'src/environments/environment';
import { AuthGuard } from './core/auth.guard';
import { ResourcesService } from 'src/app/core/resources.service';

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
    private changeDetectorRef: ChangeDetectorRef,
    public textMessages: ResourcesService
  ) {
    titleService.setTitle(environment.siteName);
  }

  public ngAfterViewChecked(): void {
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
