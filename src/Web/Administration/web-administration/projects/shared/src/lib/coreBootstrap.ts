import { Injector } from '@angular/core';

import { LoggerService } from './helpers';
import { CoreConfig } from './core.config';
import { AuthenticationService } from './authentication';
import { LocalizationService } from './localization';
import { ThemeService } from './theme';
import { LayoutService } from './layout';

export class CoreBootstrap {
    public static run(injector: Injector, appRootUrl: string, callback: () => void, resolve: any, reject: any): void {
        const logger: LoggerService = injector.get(LoggerService);

        logger.Debug('CoreBootstrap running...');

        const config: CoreConfig = injector.get(CoreConfig);

        CoreBootstrap.logCurrentEnvironment(logger, config.Environment);

        CoreBootstrap.initializeAuthentication(injector);

        CoreBootstrap.initializeLocalization(injector);

        CoreBootstrap.initializeTheme(injector);

        CoreBootstrap.initializeLayout(injector);

        callback();

        logger.Debug('CoreBootstrap finished...');
    }

    private static initializeAuthentication(injector: Injector): void {
        const authenticationService: AuthenticationService = injector.get(AuthenticationService);

        authenticationService.init();
    }

    private static initializeLocalization(injector: Injector): void {
        const localizationService: LocalizationService = injector.get(LocalizationService);

        localizationService.init();
    }

    private static initializeTheme(injector: Injector): void {
        const themeService: ThemeService = injector.get(ThemeService);

        themeService.init();
    }

    private static initializeLayout(injector: Injector): void {
        const layoutService: LayoutService = injector.get(LayoutService);

        layoutService.init();
    }

    private static logCurrentEnvironment(logger: LoggerService, env: any): void {
        if (env.production) {
            logger.Debug('Env - Production');
        } else {
            logger.Debug('Env - Development');
        }
    }
}
