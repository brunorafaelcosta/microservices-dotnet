import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DirectivesModule } from 'shared';

import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardComponent } from './dashboard.component';
import { DefaultDashboardComponent, PrivateComponent } from './default';

@NgModule({
  declarations: [
    DashboardComponent,
    DefaultDashboardComponent,
    PrivateComponent,
  ],
  imports: [
    CommonModule,
    DashboardRoutingModule,

    DirectivesModule,
  ],
  providers: [],
})
export class DashboardModule {
}
