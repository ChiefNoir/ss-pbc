import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminEditAccountsComponent } from './edit-account/admin-edit-accounts.component';
import { AdminEditCategoriesComponent } from './edit-category/admin-edit-categories.component';
import { AdminEditIntroductionComponent } from './edit-introduction/admin-edit-introduction.component';
import { AdminEditProjectsComponent } from './edit-project/admin-edit-projects.component';
import { AdminInformationComponent } from './information/admin-information.component';
import { AdminComponent } from './admin/admin.component';

import { AdminRoutingModule } from './admin.routing.module';

import { AdminMenuComponent } from './shared/admin-menu/admin-menu.component';
import { ButtonComponent } from './shared/button/button.component';
import { DialogEditProjectComponent } from './dialog-edit-project/dialog-edit-project.component';
import { DialogEditorCategoryComponent } from './dialog-editor-category/dialog-editor-category.component';
import { DialogEditAccountComponent } from './dialog-edit-account/dialog-edit-account.component';
import { FileUploaderComponent } from './shared/file-uploader/file-uploader.component';

import { SharedModule } from '../shared/shared.module';

@NgModule({
  imports: [CommonModule, AdminRoutingModule, SharedModule],
  declarations: [
    AdminMenuComponent,

    AdminComponent,
      AdminEditAccountsComponent,
      AdminEditCategoriesComponent,
      AdminEditIntroductionComponent,
      AdminEditProjectsComponent,
      AdminInformationComponent,

      ButtonComponent,
      FileUploaderComponent,
      DialogEditProjectComponent,
      DialogEditorCategoryComponent,
      DialogEditAccountComponent
    ],
})
export class AdminModule {}
