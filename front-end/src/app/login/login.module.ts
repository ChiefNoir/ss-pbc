import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login/login.component';
import { LoginRoutingModule } from './login.routing.module';

import { SharedModule } from '../shared/shared.module';

import { ButtonComponent } from './shared/button/button.component'

@NgModule({
  imports: [CommonModule, LoginRoutingModule, SharedModule],
  declarations: [LoginComponent, ButtonComponent],
})
export class LoginModule { }
