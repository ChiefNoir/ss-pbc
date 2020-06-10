import { NgModule } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule  } from '@angular/material/input';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';

const Material = [
  MatSidenavModule,
  MatToolbarModule,
  MatButtonModule,
  MatDividerModule,
  MatFormFieldModule,
  MatInputModule,
  FormsModule,
  ReactiveFormsModule
];

@NgModule({
  imports: Material,
  exports: Material,
})

export class MaterialModules {}
