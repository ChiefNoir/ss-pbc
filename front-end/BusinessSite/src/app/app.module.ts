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

import { ButtonCategoryComponent } from './component/button-category/button-category.component';
import { ButtonExternalUrlComponent } from './component/button-external-url/button-external-url.component';
import { FilterCategoryComponent } from './component/filter-category/filter-category.component';
import { LoadingComponent } from './component/loading/loading.component';
import { PaginatorComponent } from './component/paginator/paginator.component';
import { ErrorComponent } from './component/error/error.component';
import { NewsComponent } from './component/news/news.component';
import { ProjectPreviewComponent } from './component/project-preview/project-preview.component';
import { ProjectFullComponent } from './component/project-full/project-full.component';

import { OnlyIntModule } from './directive/onlyInt/onlyInt.module';

import { HomeComponent } from './view/home/home.component';
import { ProjectsListComponent } from './view/projects-list/projects-list.component';
import { NotFoundComponent } from './view/notfound/notfound.component';
import { ProjectComponent } from './view/project/project.component';


import { DataService } from './service/data.service';


@NgModule({
  declarations: [
    AppComponent,
    ButtonCategoryComponent,
    ButtonExternalUrlComponent,
    FilterCategoryComponent,
    LoadingComponent,
    ErrorComponent,
    NewsComponent,
    PaginatorComponent,
    ProjectPreviewComponent,
    ProjectFullComponent,
    HeaderComponent,
    FooterComponent,
    HomeComponent,
    ProjectsListComponent,
    NotFoundComponent,
    ProjectComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    FlexLayoutModule,
    MaterialModules,
    HttpClientModule,

    OnlyIntModule
  ],
  providers: [DataService],
  bootstrap: [AppComponent]
})
export class AppModule { }
