import { Component, Output, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';
import { ResourcesService } from '../../core/resources.service';
import { AuthGuard } from '../../core/auth.guard';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss'],
})
export class NavigationComponent {
  constructor(
    public textMessages: ResourcesService,
    public router: Router,
    public authGuard: AuthGuard
  ) {}

  @Output()
  public ShowSideNavigationClick: EventEmitter<void> = new EventEmitter<void>();

  public testing: boolean = false;

  public menuClick() {
    this.ShowSideNavigationClick.emit();
  }
}
