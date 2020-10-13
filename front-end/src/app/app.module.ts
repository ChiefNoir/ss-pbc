import { NgModule } from '@angular/core';
import { BrowserModule, Title } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FlexLayoutModule } from '@angular/flex-layout';
import { HttpClientModule } from '@angular/common/http';
import { DatePipe } from '@angular/common';
// -----

import { AppComponent } from './app.component';

// Routing
import { AppRoutingModule } from './app-routing.module';
import { IntroductionRoutingModule } from './introduction/introduction-routing.module';

import { MaterialModules } from './#import/material.modules';
// -----

// components
import { HeaderComponent } from './component/header/header.component';
import { FooterComponent } from './component/footer/footer.component';
import { ButtonComponent } from './component/button/button.component';
import { ButtonContactComponent } from './shared/button-contact.component';
import { CarouselComponent } from './project/carousel/carousel.component';
import { ButtonCategoryComponent } from './component/button-category/button-category.component';
import { ButtonExternalUrlComponent } from './shared/button-external-url.component';
import { FilterCategoryComponent } from './projects/filter-category/filter-category.component';
import { PaginatorComponent } from './component/paginator/paginator.component';
import { ProjectPreviewComponent } from './projects/project-preview/project-preview.component';
import { ProjectFullComponent } from './project/project-full/project-full.component';
import { ProjectComponent } from './project/project.component';
import { DialogEditProjectComponent } from './component/dialog-edit-project/dialog-edit-project.component';
import { DialogEditorCategoryComponent } from './component/dialog-editor-category/dialog-editor-category.component';
import { DialogEditAccountComponent } from './component/dialog-edit-account/dialog-edit-account.component';
import { MessageComponent } from './component/message/message.component';
import { FileUploaderComponent } from './component/file-uploader/file-uploader.component';
// -----

import { OnlyIntModule } from './shared/only-int.module';
// -----

import { SplitPipe } from './shared/split.pipe';
//

// views
import { IntroductionComponent } from './introduction/introduction/introduction.component';
import { ProjectsListComponent } from './projects/project-list/projects-list.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { AdminLoginComponent } from './view/admin-login/admin-login.component';
import { AdminEditProjectsComponent } from './view/admin-edit-projects/admin-edit-projects.component';
import { AdminEditCategoriesComponent } from './view/admin-edit-categories/admin-edit-categories.component';
import { AdminEditAccountsComponent } from './view/admin-edit-accounts/admin-edit-accounts.component';
import { AdminEditIntroductionComponent } from './view/admin-edit-introduction/admin-edit-introduction.component';
import { AdminComponent } from './view/admin/admin.component';
// -----

// services
import { DataService } from './core/data.service';
import { AuthService } from './core/auth.service';
import { StorageService } from './core/storage.service';
// -----

import { AuthGuard } from './core/auth.guard';

// providers
import { CookieService } from 'ngx-cookie-service';
// -----

@NgModule({
  declarations: [
    AppComponent,
    ButtonCategoryComponent,
    ButtonComponent,
    ButtonContactComponent,
    CarouselComponent,
    ButtonExternalUrlComponent,
    FilterCategoryComponent,
    PaginatorComponent,
    ProjectPreviewComponent,
    ProjectComponent,
    ProjectFullComponent,
    DialogEditProjectComponent,
    DialogEditorCategoryComponent,
    DialogEditAccountComponent,
    HeaderComponent,
    FooterComponent,
    IntroductionComponent,
    ProjectsListComponent,
    NotFoundComponent,
    ProjectComponent,
    AdminComponent,
    AdminLoginComponent,
    AdminEditProjectsComponent,
    AdminEditCategoriesComponent,
    AdminEditAccountsComponent,
    AdminEditIntroductionComponent,
    MessageComponent,
    FileUploaderComponent,
    SplitPipe,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    IntroductionRoutingModule,
    BrowserAnimationsModule,
    FlexLayoutModule,
    MaterialModules,
    HttpClientModule,
    OnlyIntModule,
  ],
  providers: [
    DatePipe,
    DataService,
    AuthService,
    StorageService,
    CookieService,
    AuthGuard,
    Title,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
