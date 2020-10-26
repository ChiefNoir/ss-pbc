import { Component, Output, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';
import { ResourcesService } from '../../resources.service';
import { AuthGuard } from '../../auth.guard';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss'],
})
export class NavigationComponent {
  public siteName: string = environment.siteName;

  constructor(
    public textMessages: ResourcesService,
    public router: Router,
    public authGuard: AuthGuard
  ) {}

  @Output()
  public ShowSideNavigationClick: EventEmitter<void> = new EventEmitter<void>();

  public menuClick() {
    this.ShowSideNavigationClick.emit();
  }
}
