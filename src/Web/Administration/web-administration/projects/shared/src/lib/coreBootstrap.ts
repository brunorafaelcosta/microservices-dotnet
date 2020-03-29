import { Injector } from '@angular/core';

import { CoreConfig } from './core.config';
import { LayoutService } from './layout';
import { ThemeService } from './theme';

export class CoreBootstrap {
    public static run(injector: Injector, appRootUrl: string, callback: () => void, resolve: any, reject: any): void {
        console.log('CoreBootstrap running...');

        const config: CoreConfig = injector.get(CoreConfig);

        CoreBootstrap.logCurrentEnvironment(config.Environment);

        CoreBootstrap.initializeLayout(injector);

        CoreBootstrap.initializeTheme(injector);

        callback();
        console.log('CoreBootstrap finished...');
    }

    private static initializeLayout(injector: Injector): void {
        const layoutService: LayoutService = injector.get(LayoutService);

        layoutService.init();
    }

    private static initializeTheme(injector: Injector): void {
        const themeService: ThemeService = injector.get(ThemeService);

        themeService.init();
    }

    private static logCurrentEnvironment(env: any): void {
        if (env.production) {
            console.log('Env - Production');
        } else {
            console.log('Env - Development');
        }
    }
}
