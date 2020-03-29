import { Routes } from '@angular/router';

import { CoreConfig, DefaultLayoutComponent } from 'shared';
import { environment } from './environments/environment';

export default class RootConfig extends CoreConfig {
    constructor() {
        super('base-admin', 'Base Administration');
    }

    public Environment: any = environment;

    public Routes: Routes = [
        {
            path: 'dashboard',
            component: DefaultLayoutComponent,
            loadChildren: () => import(`./dashboard/dashboard.module`).then(m => m.DashboardModule)
        },
        { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
    ];
}
