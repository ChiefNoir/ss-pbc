import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthGuard } from '../../../auth.guard';
import { ResourcesService } from '../../../resources.service';

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
