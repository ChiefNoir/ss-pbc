import { NgModule } from '@angular/core';

// Providers
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
//

import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { TextFieldModule } from '@angular/cdk/text-field';

import { MatDialogModule } from '@angular/material/dialog';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatTableModule } from '@angular/material/table';
import { MatTabsModule } from '@angular/material/tabs';
import { MatToolbarModule } from '@angular/material/toolbar';

const Providers = [MatDatepickerModule, MatNativeDateModule];

const Material = [
  FormsModule,
  ReactiveFormsModule,

  TextFieldModule,

  MatDatepickerModule,
  MatDialogModule,
  MatDividerModule,
  MatFormFieldModule,
  MatInputModule,
  MatNativeDateModule,
  MatSelectModule,
  MatSidenavModule,
  MatTableModule,
  MatTabsModule,
  //MatToolbarModule,
];

@NgModule({
  imports: Material,
  exports: Material,
  providers: Providers,
})
export class MaterialModules {}
