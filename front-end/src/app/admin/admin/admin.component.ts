import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { AuthGuard } from '../../auth.guard';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss'],
})
export class AdminComponent implements OnInit {
  public constructor(private router: Router, private authGuard: AuthGuard) {}

  public ngOnInit(): void {
    if (!this.authGuard.isLoggedIn()) {
      this.router.navigate(['/login']);
      this.authGuard.logoutComplete();
    }
  }
}
