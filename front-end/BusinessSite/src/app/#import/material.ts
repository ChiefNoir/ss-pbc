import { NgModule } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';

const Material = [
  MatSidenavModule,
  MatToolbarModule,
  MatButtonModule,
  MatDividerModule,
];

@NgModule({
  imports: Material,
  exports: Material,
})

export class MaterialModules {}
