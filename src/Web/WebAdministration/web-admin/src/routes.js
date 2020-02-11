import React, { lazy } from 'react';
import { Redirect } from 'react-router-dom';

import config from './config'
import navigationConfig from './navigationConfig'

// Layouts
import DefaultLayout from './__web-admin-common/layouts/Default';
import ErrorLayout from './__web-admin-common/layouts/Error';

// Page routes
import DashboardRoutes from './Dashboard/routes';

const routes = [
  {
    path: '/',
    exact: true,
    private: true,
    component: () => <Redirect to='/dashboards' />
  },
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
        component: lazy(() => import('./__web-admin-common/views/Error401'))
      },
      {
        path: '/errors/error-404',
        exact: true,
        component: lazy(() => import('./__web-admin-common/views/Error404'))
      },
      {
        path: '/errors/error-500',
        exact: true,
        component: lazy(() => import('./__web-admin-common/views/Error500'))
      },
      {
        component: () => <Redirect to={config.route.onNotFoundRouteRedirectToPath} />
      }
    ]
  },
  {
    path: '/dashboards',
    private: true,
    permissions: [],
    component: (props) => <DefaultLayout {...props} navigationConfig={navigationConfig} />,
    routes: DashboardRoutes('/dashboards')
  }
];

export default routes;
