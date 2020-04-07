import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { PermissionGuard } from 'shared';

import { DashboardComponent } from './dashboard.component';
import { DefaultDashboardComponent } from './default';

const routes: Routes = [
  {
    path: '',
    component: DashboardComponent,
    children: [
      {
        path: 'default', component: DefaultDashboardComponent,
        canActivate: [PermissionGuard],
        data: { permissions: 'permission1' },
      },
      {
        path: '', redirectTo: 'default', pathMatch: 'full'
      },
    ],
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DashboardRoutingModule {
}
