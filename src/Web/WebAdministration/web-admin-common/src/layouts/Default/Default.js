import React, { Suspense } from 'react'
import PropTypes from 'prop-types'
import { compose } from 'recompose'
import { withStyles } from '@material-ui/styles'
import { LinearProgress } from '@material-ui/core'

import customRenderRoutes from 'web-common/src/utils/customRenderRoutes'
import { NavBar, TopBar } from '../../components';

const styles = () => ({
  root: {
    height: '100%',
    width: '100%',
    display: 'flex',
    flexDirection: 'column',
    overflow: 'hidden'
  },
  topBar: {
    zIndex: 2,
    position: 'relative'
  },
  container: {
    display: 'flex',
    flex: '1 1 auto',
    overflow: 'hidden'
  },
  navBar: {
    zIndex: 3,
    width: 256,
    minWidth: 256,
    flex: '0 0 auto'
  },
  content: {
    overflowY: 'auto',
    flex: '1 1 auto'
  }
})

class Default extends React.Component {
  static propTypes = {
    navigationConfig: PropTypes.array,
    route: PropTypes.object
  }

  constructor(props) {
    super(props)

    this.state = { isMobileNavBarOpen: false }

    this.handleNavBarMobileOpen = this.handleNavBarMobileOpen.bind(this)
    this.handleNavBarMobileClose = this.handleNavBarMobileClose.bind(this)
  }

  handleNavBarMobileOpen() {
    this.setState({ isMobileNavBarOpen: true })
  }

  handleNavBarMobileClose() {
    // console.log('close');
    this.setState({ isMobileNavBarOpen: false })
  }

  render() {
    const { classes, route, navigationConfig } = this.props
    const { isMobileNavBarOpen } = this.state
    console.log('isMobileNavBarOpen', isMobileNavBarOpen)

    return (
      <div className={classes.root}>
        <TopBar
          className={classes.topBar}
          onOpenNavBarMobile={this.handleNavBarMobileOpen}
        />
        <div className={classes.container}>
          <NavBar
            navigationConfig={navigationConfig}
            className={classes.navBar}
            onMobileClose={this.handleNavBarMobileClose}
            openMobile={isMobileNavBarOpen}
          />
          <main className={classes.content}>
            <Suspense fallback={<LinearProgress color="secondary" />}>
              {customRenderRoutes(route.routes)}
            </Suspense>
          </main>
        </div>
      </div>
    )
  }
}

export default compose(
  withStyles(styles)
)(Default)
