import { NgModule } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule  } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { MatTabsModule } from '@angular/material/tabs';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { TextFieldModule } from '@angular/cdk/text-field';

const Providers = [
  MatDatepickerModule,
  MatNativeDateModule
]

const Material = [
  MatSidenavModule,
  MatToolbarModule,
  MatDividerModule,
  MatDialogModule,
  MatFormFieldModule,
  MatTableModule,
  MatInputModule,
  FormsModule,
  ReactiveFormsModule,
  MatTabsModule,
  MatSelectModule,
  TextFieldModule,
  MatDatepickerModule,
  MatNativeDateModule
];

@NgModule({
  imports: Material,
  exports: Material,
  providers: Providers
})

export class MaterialModules {}
