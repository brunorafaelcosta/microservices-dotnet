import { ModuleWithProviders, NgModule, APP_INITIALIZER, Injector, Optional, SkipSelf } from '@angular/core';
import { CommonModule, PlatformLocation } from '@angular/common';
import { RouterModule, ROUTES } from '@angular/router';

import { CoreConfig } from './core.config';
import { GetDefaultRoutes } from './core.routes';
import { CoreInitializerFactory } from './core.module-Initializer-factory';
import { AuthenticationModule, AuthenticationConfig } from './authentication';
import { LocalizationModule, LocalizationConfig } from './localization';
import { ThemeModule, ThemeConfig } from './theme';
import { LayoutModule, LayoutConfig } from './layout';

// Pages
import {
  Error401Component,
  Error404Component,
  Error500Component,
} from './pages/error';

@NgModule({
  declarations: [
    Error401Component,
    Error404Component,
    Error500Component,
  ],
  imports: [
    CommonModule,

    RouterModule.forRoot([]),

    AuthenticationModule,
    LocalizationModule,
    ThemeModule,
    LayoutModule,
  ],
})
export class CoreModule {
  constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
    if (parentModule) {
      throw new Error(
        'CoreModule has already been loaded. Import it in the RootModule only.'
      );
    }
  }

  static forRoot(config: CoreConfig): ModuleWithProviders<CoreModule> {
    const coreRoutes = GetDefaultRoutes(config);
    
    return <ModuleWithProviders<CoreModule>>{
      ngModule: CoreModule,
      providers: [
        {
          provide: ROUTES,
          useValue: [...config.Routes, ...coreRoutes],
          multi: true,
        },
        { provide: CoreConfig, useValue: config },
        { provide: AuthenticationConfig, useValue: config.AuthenticationConfig },
        { provide: LocalizationConfig, useValue: config.LocalizationConfig },
        { provide: ThemeConfig, useValue: config.ThemeConfig },
        { provide: LayoutConfig, useValue: config.LayoutConfig },
        {
          provide: APP_INITIALIZER,
          useFactory: CoreInitializerFactory,
          deps: [Injector, PlatformLocation],
          multi: true,
        },
      ],
    };
  }
}
