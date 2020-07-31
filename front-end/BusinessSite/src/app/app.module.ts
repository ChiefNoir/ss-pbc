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
import { PaginatorComponent } from './component/paginator/paginator.component';
import { ProjectPreviewComponent } from './component/project-preview/project-preview.component';
import { ProjectFullComponent } from './component/project-full/project-full.component';
import { ProjectEditorComponent } from './component/project-editor/project-editor.component';
import { DialogEditorCategoryComponent } from './component/dialog-editor-category/dialog-editor-category.component';
import { DialogEditAccountComponent } from './component/dialog-edit-account/dialog-edit-account.component';
import { SpinnerTextComponent } from './component/spinner-text/spinner-text.component';
import { MessageComponent } from './component/message/message.component';
import { FileUploaderComponent } from './component/file-uploader/file-uploader.component';
// -----

import { OnlyIntModule } from './directive/onlyInt/onlyInt.module';
// -----

import { SplitPipe } from './pipe/SplitPipe';
//

// views
import { IntroductionComponent } from './view/introduction/introduction.component';
import { ProjectsListComponent } from './view/projects-list/projects-list.component';
import { NotFoundComponent } from './view/notfound/notfound.component';
import { ProjectComponent } from './view/project/project.component';
import { AdminLoginComponent } from './view/admin-login/admin-login.component';
import { AdminEditProjectsComponent } from './view/admin-edit-projects/admin-edit-projects.component';
import { AdminEditCategoriesComponent } from './view/admin-edit-categories/admin-edit-categories.component';
import { AdminEditAccountsComponent } from './view/admin-edit-accounts/admin-edit-accounts.component';
import { AdminEditIntroductionComponent } from './view/admin-edit-introduction/admin-edit-introduction.component';
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
    PaginatorComponent,
    ProjectPreviewComponent,
    ProjectFullComponent,
    ProjectEditorComponent,
    DialogEditorCategoryComponent,
    DialogEditAccountComponent,
    HeaderComponent,
    FooterComponent,
    IntroductionComponent,
    ProjectsListComponent,
    NotFoundComponent,
    ProjectComponent,
    AdminLoginComponent,
    AdminEditProjectsComponent,
    AdminEditCategoriesComponent,
    AdminEditAccountsComponent,
    AdminEditIntroductionComponent,
    SpinnerTextComponent,
    MessageComponent,
    FileUploaderComponent,
    SplitPipe
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
