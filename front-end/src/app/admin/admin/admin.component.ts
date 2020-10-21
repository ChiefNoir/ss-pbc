import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { Title } from '@angular/platform-browser';
import { environment } from 'src/environments/environment';
import { AuthGuard } from '../../core/auth.guard';
import { ResourcesService } from 'src/app/core/resources.service';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss'],
})
export class AdminComponent implements OnInit {
  public constructor(
    titleService: Title,
    resources: ResourcesService,
    private router: Router,
    private authGuard: AuthGuard
  ) {
    titleService.setTitle(resources.TitleAdmin + environment.siteName);
  }

  public async ngOnInit(): Promise<void> {
    if (!this.authGuard.isLoggedIn()) {
      this.router.navigate(['/login']);
      this.authGuard.logoutComplete();
    }
  }
}
