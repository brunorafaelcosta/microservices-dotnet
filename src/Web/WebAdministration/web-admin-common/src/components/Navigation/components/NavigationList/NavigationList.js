/* eslint-disable react/no-multi-comp */
import React from 'react'
import { matchPath } from 'react-router-dom'
import PropTypes from 'prop-types'
import { List } from '@material-ui/core'

import { NavigationListItem } from '../'

class NavigationList extends React.Component {
    static propTypes = {
        router: PropTypes.object,
        depth: PropTypes.number,
        pages: PropTypes.array
    }

    constructor(props) {
        super(props)

        this.reduceChildRoutes = this.reduceChildRoutes.bind(this)
    }

    reduceChildRoutes(custonProps) {
        const { items, page, depth, router } = custonProps

        if (page.children) {
            const open = matchPath(router.location.pathname, {
                path: page.href,
                exact: false
            })

            items.push(
                <NavigationListItem
                    depth={depth}
                    icon={page.icon}
                    key={page.title}
                    label={page.label}
                    open={Boolean(open)}
                    title={page.title}
                    permissions={page.permissions}
                >
                    <NavigationList
                        depth={depth + 1}
                        pages={page.children}
                        router={router}
                    />
                </NavigationListItem>
            )
        } else {
            items.push(
                <NavigationListItem
                    depth={depth}
                    href={page.href}
                    icon={page.icon}
                    key={page.title}
                    label={page.label}
                    title={page.title}
                    permissions={page.permissions}
                />
            )
        }

        return items
    }

    render() {
        const { pages, ...rest } = this.props

        return (
            <List>
                {pages.reduce(
                    (items, page) => this.reduceChildRoutes({ items, page, ...rest }),
                    []
                )}
            </List>
        )
    }
}

export default NavigationList
