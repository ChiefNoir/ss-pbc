import { NgModule } from '@angular/core';
import { BrowserModule, Title } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { HttpClientModule } from '@angular/common/http';
import { DatePipe } from '@angular/common';
// -----

import { AppComponent } from './app.component';

// Routing
import { AppRoutingModule } from './app.routing.module';


// -----
import { environment } from '../environments/environment';
// components
import { HeaderComponent } from './shared/header/header.component';
import { FooterComponent } from './shared/footer/footer.component';

// -----

import { OnlyIntModule } from './shared/only-int.module';
// -----

import { SplitPipe } from './shared/split.pipe';

// services
import { PublicService } from './core/public.service';
import { PrivateService } from './core/private.service';
import { AuthService } from './core/auth.service';
import { StorageService } from './core/storage.service';
import { ResourcesService } from './core/resources.service';
// -----

import { SharedModule } from './shared/shared.module';

import { AuthGuard } from './core/auth.guard';

// providers
import { CookieService } from 'ngx-cookie-service';
// -----

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    FooterComponent,
    SplitPipe
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    OnlyIntModule,
    SharedModule,
  ],
  providers: [
    DatePipe,
    PublicService,
    PrivateService,
    AuthService,
    StorageService,
    CookieService,
    AuthGuard,
    Title,
    ResourcesService,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
