import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FlexLayoutModule } from '@angular/flex-layout';

import { MaterialModules } from './#import/material';

import { HeaderComponent } from './component/header/header.component';
import { FooterComponent } from './component/footer/footer.component';

import { LoadingComponent } from './component/loading/loading.component';
import { ErrorComponent } from './component/error/error.component';
import { NewsComponent } from './component/news/news.component';
import { ProjectPreviewComponent } from './component/project-preview/project-preview.component';



import { HomeComponent } from './view/home/home.component';
import { ProjectsListComponent } from './view/projects-list/projects-list.component';
import { NotFoundComponent } from './view/notfound/notfound.component';


import { DataService } from './service/data.service';


@NgModule({
  declarations: [
    AppComponent,
    LoadingComponent,
    ErrorComponent,
    NewsComponent,
    ProjectPreviewComponent,
    HeaderComponent,
    FooterComponent,
    HomeComponent,
    ProjectsListComponent,
    NotFoundComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    FlexLayoutModule,
    MaterialModules,
    HttpClientModule
  ],
  providers: [DataService],
  bootstrap: [AppComponent]
})
export class AppModule { }
