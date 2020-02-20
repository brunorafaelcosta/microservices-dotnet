import React, { lazy } from 'react'
import { Redirect } from 'react-router-dom'

const Routes = (basePath, moduleConfig) => {
    return [
        {
            path: `${basePath}`,
            exact: true,
            component: () => <Redirect to={`${basePath}/default`} />
        },
        {
            path: `${basePath}/default`,
            exact: true,
            // component: lazy(() => import('./DashboardDefault'))
            component: lazy(() => {
                return new Promise(resolve => {
                    setTimeout(() => resolve(import("./DashboardDefault")), 2500)
                })
            })
        },
        {
            component: () => <Redirect to={moduleConfig.router.onNotFoundRouteRedirectToPath} />
        }
    ]
}

export default Routes
