import { Routes } from '@angular/router';

import { environment } from './environments/environment';
import {
    CoreConfig,
    DefaultLayoutComponent,
    AuthenticationConfig,
} from 'shared';

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

    public DefaultRoutePath: string = '/';

    public AuthenticationConfig: AuthenticationConfig = {
        AuthorityClientId: 'web_base_administration',
        AuthorityScope: 'openid profile',
        AuthorityLoginUrl: environment.authorityLoginUrl,
        AuthorityRedirectUrl: environment.authorityRedirectUrl,
        AuthorityLogoutUrl: environment.authorityLogoutUrl,
        AuthorityUserInfoUrl: environment.authorityUserInfoUrl,
    };
}
