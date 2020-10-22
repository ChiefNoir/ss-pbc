import { Component, Output, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';
import { ResourcesService } from '../../core/resources.service';
import { AuthGuard } from '../../core/auth.guard';

@Component({
  selector: 'app-navigation-compact',
  templateUrl: './navigation-compact.component.html',
  styleUrls: ['./navigation-compact.component.scss'],
})
export class NavigationCompactComponent {
  constructor(
    public textMessages: ResourcesService,
    public router: Router,
    public authGuard: AuthGuard
  ) {}

  @Output()
  public HideClick: EventEmitter<void> = new EventEmitter<void>();

  public closeClick() {
    this.HideClick.emit();
  }
}
