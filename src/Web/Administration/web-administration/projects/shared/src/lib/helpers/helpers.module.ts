import { NgModule } from '@angular/core';

import { LoggerService } from './logger.service';
import { RouterService } from './router.service';
import { StorageService } from './storage.service';

@NgModule({
  imports: [],
  declarations: [],
  providers: [
    LoggerService,
    RouterService,
    StorageService,
  ],
})
export class HelpersModule {
}
