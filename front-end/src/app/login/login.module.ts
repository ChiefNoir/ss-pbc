import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from './auth.service';


import { LoginComponent } from './login/login.component';
import { LoginRoutingModule } from './login.routing.module';

import { SharedModule } from '../shared/shared.module';

@NgModule({
  imports: [CommonModule, LoginRoutingModule, SharedModule],
  declarations: [LoginComponent],
  providers: [AuthService]
})
export class LoginModule { }
