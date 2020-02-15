/* eslint-disable react/no-multi-comp */
import React from 'react'
import { withRouter } from 'react-router'
import { compose } from 'recompose'
import clsx from 'clsx'
import PropTypes from 'prop-types'
import { withStyles } from '@material-ui/styles'
import { Typography } from '@material-ui/core'

import { NavigationList } from './components'

const styles = theme => ({
  root: {
    marginBottom: theme.spacing(3)
  }
})

class Navigation extends React.Component {
  constructor(props) {
    super(props)
  }

  render() {
    const { classes, title, pages, className, component: Component, 
        match, location, history, staticContext, ...rest } = this.props
    
    const router = { match, location, history }

    return (
      <Component
        {...rest}
        className={clsx(classes.root, className)}
      >
        {title && <Typography variant="overline">{title}</Typography>}
        <NavigationList
          depth={0}
          pages={pages}
          router={router}
        />
      </Component>
    )
  }
}
Navigation.propTypes = {
  className: PropTypes.string,
  component: PropTypes.any,
  pages: PropTypes.array.isRequired,
  title: PropTypes.string
}
Navigation.defaultProps = {
  component: 'nav'
}

export default compose(
  withStyles(styles),
  withRouter
)(Navigation)
