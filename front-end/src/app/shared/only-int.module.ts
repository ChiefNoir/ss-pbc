import { NgModule } from '@angular/core';
import { OnlyIntDirective } from './only-int.directive';

@NgModule({
  declarations: [OnlyIntDirective],
  exports: [OnlyIntDirective],
})

export class OnlyIntModule {}
