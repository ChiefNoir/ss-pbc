import { NgModule } from '@angular/core';

import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSidenavModule } from '@angular/material/sidenav';

const Material = [
  MatDividerModule,
  MatFormFieldModule,
  MatInputModule,
  MatSidenavModule
];

@NgModule({
  imports: Material,
  exports: Material,
})
export class MaterialModule {}
