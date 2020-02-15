/* eslint-disable no-unused-vars */
import React, { useState, useRef, useEffect } from 'react'
import { Link as RouterLink } from 'react-router-dom'
import clsx from 'clsx'
import PropTypes from 'prop-types'
import { compose } from 'recompose'
import { withStyles } from '@material-ui/styles'
import {
  AppBar,
  Button,
  IconButton,
  Toolbar,
  Hidden,
} from '@material-ui/core'
import InputIcon from '@material-ui/icons/Input'
import MenuIcon from '@material-ui/icons/Menu'

import { withTranslation } from 'react-i18next'

import { AppContext } from 'web-common/src/context'

const styles = theme => ({
  root: {
    boxShadow: 'none'
  },
  flexGrow: {
    flexGrow: 1
  },
  logoutButton: {
    marginLeft: theme.spacing(1)
  },
  logoutIcon: {
    marginRight: theme.spacing(1)
  }
})

class TopBar extends React.Component {
  constructor(props) {
    super(props)
  }

  render() {
    const { classes, onOpenNavBarMobile, className, t, tReady, ...rest } = this.props

    return (
      <AppContext.Consumer>
        {app => (
          <AppBar
            {...rest}
            className={clsx(classes.root, className)}
            color="primary"
          >
            <Toolbar>
              <RouterLink to="/">
                Company name
                {/* <img
                  width="200px"
                  alt="Logo"
                  src="/images/logos/logo.png"
                /> */}
              </RouterLink>
              <div className={classes.flexGrow} />
              <Hidden mdDown>
                <Button
                  className={classes.logoutButton}
                  color="inherit"
                  onClick={app.logoutUser}
                >
                  <InputIcon className={classes.logoutIcon} />
                  {t('signout.label')}
                </Button>
              </Hidden>
              <Hidden lgUp>
                <IconButton
                  color="inherit"
                  onClick={onOpenNavBarMobile}
                >
                  <MenuIcon />
                </IconButton>
              </Hidden>
            </Toolbar>
          </AppBar>
        )}
      </AppContext.Consumer>
    )
  }
}

TopBar.propTypes = {
  className: PropTypes.string,
  onOpenNavBarMobile: PropTypes.func
}

export default compose(
  withStyles(styles),
  withTranslation()
)(TopBar)
