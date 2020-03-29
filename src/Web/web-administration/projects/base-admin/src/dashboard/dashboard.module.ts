import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardComponent } from './dashboard.component';
import { DefaultDashboardComponent } from './default';

@NgModule({
  declarations: [
    DashboardComponent,
    DefaultDashboardComponent,
  ],
  imports: [
    CommonModule,
    DashboardRoutingModule,
  ],
  providers: [],
})
export class DashboardModule {
}
