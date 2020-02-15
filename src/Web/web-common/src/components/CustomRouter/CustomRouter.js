import React from 'react'
import { Switch, Route } from "react-router"
import { Redirect } from 'react-router-dom'

import { ConfigContext, AppContext } from '../../context'

function CustomRouter(props) {
    const { routes, extraProps = {}, switchProps = {} } = props

    return routes
        ?
        (
            <ConfigContext.Consumer>
                {configCtx => (
                    <AppContext.Consumer>
                        {appCtx => (
                            <Switch {...switchProps}>
                                {routes.map((route, i) => (
                                    <Route
                                        key={route.key || i}
                                        path={route.path}
                                        exact={route.exact}
                                        strict={route.strict}
                                        render={props => {
                                            const requiredPermissions = (route.permissions) ? route.permissions : []
                                            if (route.private || requiredPermissions.length > 0) {
                                                const isRestricted = !appCtx || !appCtx.authentication || !appCtx.authentication.loggedIn
                                                if (isRestricted) {
                                                    return (<Redirect to={configCtx.router.onUnauthorizedRouteRedirectToPath} />)
                                                }
                                            }

                                            return (route.render
                                                ? (route.render({ ...props, ...extraProps, route: route }))
                                                : (<route.component {...props} {...extraProps} route={route} />)
                                            )
                                        }}
                                    />
                                ))}
                            </Switch>
                        )}
                    </AppContext.Consumer>
                )}
            </ConfigContext.Consumer>
        )
        :
        null
}

export default CustomRouter