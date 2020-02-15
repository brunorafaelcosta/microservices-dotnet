import React from 'react'
import { Redirect } from 'react-router-dom'

// Layouts
import DefaultLayout from 'web-admin-common/src/layouts/Default'

import { moduleNavigation } from './'

// Page routes
import { Routes as DashboardRoutes } from '../Dashboard';

const moduleRoutes = (config) => {
  return [
    {
      path: '/',
      exact: true,
      private: true,
      component: () => <Redirect to='/dashboards' />
    },
    {
      path: '/dashboards',
      private: true,
      permissions: [],
      component: (props) => <DefaultLayout {...props} navigationConfig={moduleNavigation} />,
      routes: DashboardRoutes('/dashboards', config)
    }
  ];
}

export default moduleRoutes
