import { NgModule } from '@angular/core';

import { HasPermissionDirective } from '.';

@NgModule({
  declarations: [
    HasPermissionDirective,
  ],
  exports: [
    HasPermissionDirective,
  ],
})
export class DirectivesModule {
}
