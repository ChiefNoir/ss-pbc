import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthGuard } from 'src/app/core/auth.guard';
import { ResourcesService } from 'src/app/core/resources.service';

@Component({
  selector: 'app-admin-menu',
  templateUrl: './admin-menu.component.html',
  styleUrls: ['./admin-menu.component.scss'],
})
export class AdminMenuComponent {
  constructor(
    public authGuard: AuthGuard,
    public textMessages: ResourcesService,
    public router: Router
  ) {}
}
