import { Routes } from '@angular/router';

import { AuthenticationConfig } from './authentication';
import { LocalizationConfig } from './localization';
import { ThemeConfig } from './theme';
import { LayoutConfig } from './layout';

export class CoreConfig {
    constructor(appName: string, appTitle: string = appName) {
        this.AppName = appName;
        this.AppTitle = appTitle;
    }
    
    public AppName: string;
    public AppTitle: string;

    public Environment: any;

    public Routes: Routes;
    public OnNotFoundRouteRedirectToPath: string = 'errors/error-404';
    public OnUnauthorizedRouteRedirectToPath: string = 'errors/error-401';

    public AuthenticationConfig: AuthenticationConfig;

    public LocalizationConfig: LocalizationConfig;

    public ThemeConfig: ThemeConfig;

    public LayoutConfig: LayoutConfig;
}
