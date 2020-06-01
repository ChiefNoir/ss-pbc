import { NgModule } from '@angular/core';
import { OnlyIntDirective } from './onlyInt.directive';

@NgModule({
  declarations: [OnlyIntDirective],
  exports: [OnlyIntDirective]
})

export class OnlyIntModule {}
