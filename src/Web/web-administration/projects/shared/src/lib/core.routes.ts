import { Routes } from '@angular/router';

import { CoreConfig } from './core.config';
import { ErrorLayoutComponent } from './layout';
import {
  Error401Component,
  Error404Component,
  Error500Component
} from './pages/error';

export function GetDefaultRoutes(config: CoreConfig): Routes {
  const onNotFoundRouteRedirectToPath =
    config.OnNotFoundRouteRedirectToPath || 'errors/error-404';

  return [
    {
      path: 'errors',
      component: ErrorLayoutComponent,
      children: [
        {
          path: 'error-401',
          component: Error401Component,
        },
        {
          path: 'error-404',
          component: Error404Component,
        },
        {
          path: 'error-500',
          component: Error500Component,
        },
      ],
    },

    { path: '**', redirectTo: onNotFoundRouteRedirectToPath, pathMatch: 'full' },
  ];
}
