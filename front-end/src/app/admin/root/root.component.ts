import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { AuthGuard } from '../../shared/services/auth.guard';

@Component({
  selector: 'app-root',
  templateUrl: './root.component.html',
  styleUrls: ['./root.component.scss'],
})
export class RootComponent implements OnInit {
  public constructor(
    private router: Router,
    private authGuard: AuthGuard
    ) {}

  public ngOnInit(): void {
    if (!this.authGuard.isLoggedIn()) {
      this.router.navigate(['/login']);
      this.authGuard.logoutComplete();
    }
  }
}
