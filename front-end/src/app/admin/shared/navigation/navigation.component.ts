import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthGuard } from '../../../shared/services/auth.guard';
import { ResourcesService } from '../../../shared/services/resources.service';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss'],
})
export class NavigationComponent {
  constructor(
    public authGuard: AuthGuard,
    public textMessages: ResourcesService,
    public router: Router
  ) {}
}
