import { NgModule } from '@angular/core';

import { AuthenticationGuard, PermissionGuard } from '.';

@NgModule({
  providers: [
    AuthenticationGuard,
    PermissionGuard,
  ],
})
export class GuardsModule {
}
