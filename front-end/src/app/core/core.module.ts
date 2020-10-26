import { NgModule } from '@angular/core';

import { AuthGuard } from './services/auth.guard';
import { ResourcesService } from './services/resources.service';
import { PublicService } from './services/public.service';
import { StorageService } from './services/storage.service';

@NgModule({
    providers: [
        AuthGuard,
        ResourcesService,
        PublicService,
        StorageService
    ]
  })
  export class CoreModule {}
