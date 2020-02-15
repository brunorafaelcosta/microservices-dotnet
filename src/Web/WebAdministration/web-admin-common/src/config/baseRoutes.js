import React, { lazy } from 'react'
import { Redirect } from 'react-router-dom'

// Layouts
import DefaultLayout from 'web-admin-common/src/layouts/Default'
import ErrorLayout from 'web-admin-common/src/layouts/Error'

const routes = (config) => {
  return [
    {
      path: '/auth',
      exact: true,
      component: () => { window.location.href = config.webAuthenticationUrl; return null; }
    },
    {
      path: '/errors',
      component: ErrorLayout,
      routes: [
        {
          path: '/errors/error-401',
          exact: true,
          component: lazy(() => import('web-admin-common/src/views/Error401'))
        },
        {
          path: '/errors/error-404',
          exact: true,
          component: lazy(() => import('web-admin-common/src/views/Error404'))
        },
        {
          path: '/errors/error-500',
          exact: true,
          component: lazy(() => import('web-admin-common/src/views/Error500'))
        },
        {
          component: () => <Redirect to={config.router.onNotFoundRouteRedirectToPath} />
        }
      ]
    },
    {
      component: () => <Redirect to={config.router.onNotFoundRouteRedirectToPath} />
    },
  ];
}

export default routes
