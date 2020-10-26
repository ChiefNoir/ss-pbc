import { NgModule } from '@angular/core';
import { BrowserModule, Title } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { HttpClientModule } from '@angular/common/http';
import { DatePipe } from '@angular/common';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app.routing.module';
import { NotFoundComponent } from './not-found.component';

import { PublicService } from './public.service';
import { StorageService } from './storage.service';
import { ResourcesService } from './resources.service';
import { AuthGuard } from './auth.guard';


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
    StorageService,
    AuthGuard,
    Title,
    ResourcesService,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
