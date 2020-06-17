import { NgModule } from '@angular/core';
import { BrowserModule, Title } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FlexLayoutModule } from '@angular/flex-layout';
import { HttpClientModule } from '@angular/common/http';
// -----

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';

import { MaterialModules } from './#import/material';
// -----

// components
import { HeaderComponent } from './component/header/header.component';
import { FooterComponent } from './component/footer/footer.component';
import { ButtonComponent } from './component/button/button.component';
import { ButtonCategoryComponent } from './component/button-category/button-category.component';
import { ButtonExternalUrlComponent } from './component/button-external-url/button-external-url.component';
import { FilterCategoryComponent } from './component/filter-category/filter-category.component';
import { LoadingComponent } from './component/loading/loading.component';
import { PaginatorComponent } from './component/paginator/paginator.component';
import { ErrorComponent } from './component/error/error.component';
import { NewsComponent } from './component/news/news.component';
import { ProjectPreviewComponent } from './component/project-preview/project-preview.component';
import { ProjectFullComponent } from './component/project-full/project-full.component';
import { ProjectEditorComponent } from './component/project-editor/project-editor.component';
import { DialogEditorCategoryComponent } from './component/dialog-editor-category/dialog-editor-category.component';
// -----

import { OnlyIntModule } from './directive/onlyInt/onlyInt.module';
// -----

// views
import { HomeComponent } from './view/home/home.component';
import { ProjectsListComponent } from './view/projects-list/projects-list.component';
import { NotFoundComponent } from './view/notfound/notfound.component';
import { ProjectComponent } from './view/project/project.component';
import { AdminLoginComponent } from './view/admin-login/admin-login.component';
import { AdminProjectEditorComponent } from './view/admin-project-editor/admin-project-editor.component';
import { AdminEditCategoriesComponent } from './view/admin-edit-categories/admin-edit-categories.component';
// -----

// services
import { DataService } from './service/data.service';
import { AuthService } from './service/auth.service';
import { StorageService } from './service/storage.service';
// -----

// providers
import { CookieService } from 'ngx-cookie-service';
// -----

@NgModule({
  declarations: [
    AppComponent,
    ButtonCategoryComponent,
    ButtonComponent,
    ButtonExternalUrlComponent,
    FilterCategoryComponent,
    LoadingComponent,
    ErrorComponent,
    NewsComponent,
    PaginatorComponent,
    ProjectPreviewComponent,
    ProjectFullComponent,
    ProjectEditorComponent,
    DialogEditorCategoryComponent,
    HeaderComponent,
    FooterComponent,
    HomeComponent,
    ProjectsListComponent,
    NotFoundComponent,
    ProjectComponent,
    AdminLoginComponent,
    AdminProjectEditorComponent,
    AdminEditCategoriesComponent
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
  providers: [
    DataService,
    AuthService,
    StorageService,
    CookieService,
    Title
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
