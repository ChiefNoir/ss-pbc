import { NgModule } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';

import { MatInputModule  } from '@angular/material/input';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {MatTableModule} from '@angular/material/table';
import {MatTabsModule} from '@angular/material/tabs';
import {MatDialogModule} from '@angular/material/dialog';
import {TextFieldModule} from '@angular/cdk/text-field';

const Material = [
  MatSidenavModule,
  MatToolbarModule,
  MatButtonModule,
  MatDividerModule,
  MatDialogModule,
  MatFormFieldModule,
  MatTableModule,
  MatInputModule,
  FormsModule,
  ReactiveFormsModule,
  MatTabsModule,
  TextFieldModule
]; 

@NgModule({
  imports: Material,
  exports: Material,
})

export class MaterialModules {}
