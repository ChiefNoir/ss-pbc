import { NgModule } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatButtonModule } from '@angular/material/button';

const Material =
[
  MatSidenavModule,
  MatToolbarModule,
  MatButtonModule
];

@NgModule({
    imports: Material,
    exports: Material
  })
  export class MaterialModules {}