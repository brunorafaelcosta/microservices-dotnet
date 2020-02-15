/* eslint-disable no-unused-vars */
import React, { Fragment } from 'react'
import { compose } from 'recompose'
import clsx from 'clsx'
import PropTypes from 'prop-types'
import { withStyles } from '@material-ui/styles'
import { Drawer, Divider, Paper, Typography, Hidden } from '@material-ui/core'
import PerfectScrollbar from "react-perfect-scrollbar"
import { withTranslation } from 'react-i18next'
import { ComponentAuthorizer } from 'web-common/src/components'

import { Navigation } from '../'

const styles = theme => ({
  root: {
    height: '100%',
    overflowY: 'auto'
  },
  content: {
    padding: theme.spacing(2),
    position: "relative",
    top: '0',
    flex: '1 0 auto',
    height: '100%',
    display: 'flex',
    outline: '0',
    overflowY: 'auto',
    flexDirection: 'column',
  },
  navigation: {
    marginTop: theme.spacing(2),
  },
  divider: {
    marginTop: theme.spacing(2)
  },
  appVersion: {
    marginTop: theme.spacing(2),
    textAlign: "center"
  },
});

class NavBar extends React.Component {
  constructor(props) {
    super(props)
  }
  
  componentDidMount() {
    if (this.props.openMobile) {
      this.props.onMobileClose && this.props.onMobileClose();
    }
  }

  render() {
    const { navigationConfig, openMobile, onMobileClose, className, classes, t, tReady, ...rest } = this.props;

    return (
      <Fragment>
        <Hidden lgUp>
          <Drawer
            anchor="left"
            onClose={onMobileClose}
            open={openMobile}
            variant="temporary"
          >
            <div
              {...rest}
              className={clsx(classes.root, className)}
            >
              <div className={classes.content}>
                <PerfectScrollbar>
                  <nav className={classes.navigation}>
                    {navigationConfig.map(list => (
                      <ComponentAuthorizer Permissions={list.permissions} key={list.title}>
                        <Navigation
                          component="div"
                          key={list.title}
                          pages={list.pages}
                          title={t(list.title)}
                        />
                      </ComponentAuthorizer>
                    ))}
                  </nav>
                </PerfectScrollbar>

                <Divider className={classes.divider} />

                <div className={classes.appVersion}>
                  <Typography
                    className={classes.name}
                    variant="overline"
                  >
                    1.0.000
        </Typography>
                </div>
              </div>
            </div>
          </Drawer>
        </Hidden>
        <Hidden mdDown>
          <Paper
            {...rest}
            className={clsx(classes.root, className)}
            elevation={0}
            square
          >
            <div className={classes.content}>
              <PerfectScrollbar>
                <nav className={classes.navigation}>
                  {navigationConfig.map(list => (
                    <ComponentAuthorizer Permissions={list.permissions} key={list.title}>
                      <Navigation
                        component="div"
                        key={list.title}
                        pages={list.pages}
                        title={t(list.title)}
                      />
                    </ComponentAuthorizer>
                  ))}
                </nav>
              </PerfectScrollbar>

              <Divider className={classes.divider} />

              <div className={classes.appVersion}>
                <Typography
                  className={classes.name}
                  variant="overline"
                >
                  1.0.000
        </Typography>
              </div>
            </div>
          </Paper>
        </Hidden>
      </Fragment>
    )
  }
}

NavBar.propTypes = {
  className: PropTypes.string,
  onMobileClose: PropTypes.func,
  openMobile: PropTypes.bool,
  navigationConfig: PropTypes.array
};

export default compose(
  withStyles(styles),
  withTranslation()
)(NavBar)
