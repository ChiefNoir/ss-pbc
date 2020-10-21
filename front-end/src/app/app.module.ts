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

// components
import { HeaderComponent } from './shared/header/header.component';
import { FooterComponent } from './shared/footer/footer.component';
import { ButtonComponent } from './admin/shared/button/button.component';
// import { CarouselComponent } from './project/carousel/carousel.component';
// import { ButtonCategoryComponent } from './projects/button-category/button-category.component';
// import { ButtonExternalUrlComponent } from './shared/button-external-url/button-external-url.component';
// import { FilterCategoryComponent } from './projects/filter-category/filter-category.component';
// import { PaginatorComponent } from './shared/paginator/paginator.component';
// import { ProjectPreviewComponent } from './projects/project-preview/project-preview.component';
// import { ProjectFullComponent } from './project/project-full/project-full.component';
// import { ProjectComponent } from './project/project.component';
import { DialogEditProjectComponent } from './admin/dialog-edit-project/dialog-edit-project.component';
import { DialogEditorCategoryComponent } from './admin/dialog-editor-category/dialog-editor-category.component';
import { DialogEditAccountComponent } from './admin/dialog-edit-account/dialog-edit-account.component';

import { FileUploaderComponent } from './admin/shared/file-uploader/file-uploader.component';
import { AdminMenuComponent } from './admin/shared/admin-menu/admin-menu.component';
// -----

import { OnlyIntModule } from './shared/only-int.module';
// -----

import { SplitPipe } from './shared/split.pipe';
//

// views
// import { ProjectsListComponent } from './projects/project-list/projects-list.component';
// import { NotFoundComponent } from './not-found/not-found.component';
import { AdminLoginComponent } from './admin/login/admin-login.component';
import { AdminEditProjectsComponent } from './admin/edit-project/admin-edit-projects.component';
import { AdminEditCategoriesComponent } from './admin/edit-category/admin-edit-categories.component';
import { AdminEditAccountsComponent } from './admin/edit-account/admin-edit-accounts.component';
import { AdminEditIntroductionComponent } from './admin/edit-introduction/admin-edit-introduction.component';
import { AdminInformationComponent } from './admin/information/admin-information.component';
// -----

// services
import { PublicService } from './core/public.service';
import { PrivateService } from './core/private.service';
import { AuthService } from './core/auth.service';
import { StorageService } from './core/storage.service';
import { ResourcesService } from './core/resources.service';
import { JwtHelperService, JwtModule } from '@auth0/angular-jwt';
// -----

import { SharedModule } from './shared/shared.module';

import { AuthGuard } from './core/auth.guard';

// providers
import { CookieService } from 'ngx-cookie-service';
// -----

@NgModule({
  declarations: [
    AppComponent,
//    ButtonCategoryComponent,
    ButtonComponent,
//    CarouselComponent,
//    ButtonExternalUrlComponent,
//    FilterCategoryComponent,
//    PaginatorComponent,
//    ProjectPreviewComponent,
//    ProjectComponent,
//    ProjectFullComponent,
    DialogEditProjectComponent,
    DialogEditorCategoryComponent,
    DialogEditAccountComponent,
    HeaderComponent,
    FooterComponent,
//    ProjectsListComponent,
//    NotFoundComponent,
//    ProjectComponent,
    AdminInformationComponent,
    AdminLoginComponent,
    AdminEditProjectsComponent,
    AdminEditCategoriesComponent,
    AdminEditAccountsComponent,
    AdminEditIntroductionComponent,
    FileUploaderComponent,
    AdminMenuComponent,
    SplitPipe
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    OnlyIntModule,
    SharedModule
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
