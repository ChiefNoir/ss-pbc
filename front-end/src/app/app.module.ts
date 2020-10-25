import { NgModule } from '@angular/core';
import { BrowserModule, Title } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { HttpClientModule } from '@angular/common/http';
import { DatePipe } from '@angular/common';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app.routing.module';
import { NotFoundComponent } from './not-found.component';

import { PublicService } from './core/public.service';
import { PrivateService } from './core/private.service';
import { AuthService } from './core/auth.service';
import { StorageService } from './core/storage.service';
import { ResourcesService } from './core/resources.service';
import { AuthGuard } from './core/auth.guard';


import { SharedModule } from './shared/shared.module';

import { OnlyIntModule } from './shared/only-int.module';
import { HeaderComponent } from './shared/header/header.component';
import { FooterComponent } from './shared/footer/footer.component';
import { NavigationComponent } from './shared/navigation/navigation.component';
import { NavigationCompactComponent } from './shared/navigation-compact/navigation-compact.component';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    FooterComponent,
    NotFoundComponent,
    NavigationComponent,
    NavigationCompactComponent
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
    AuthGuard,
    Title,
    ResourcesService,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
