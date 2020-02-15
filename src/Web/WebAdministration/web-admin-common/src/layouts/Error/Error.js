import React, { Suspense } from 'react'
import PropTypes from 'prop-types'
import { compose } from 'recompose'
import { withStyles } from '@material-ui/styles'
import { LinearProgress } from '@material-ui/core'

import customRenderRoutes from 'web-common/src/utils/customRenderRoutes'

const styles = () => ({
  root: {
    height: '100%'
  }
})

class Error extends React.Component {
  static propTypes = {
    route: PropTypes.object
  }

  constructor(props) {
    super(props)
  }

  render() {
    const { classes, route } = this.props

    return (
      <main className={classes.root}>
        <Suspense fallback={<LinearProgress />}>
          {customRenderRoutes(route.routes)}
        </Suspense>
      </main>
    )
  }
}

export default compose(
  withStyles(styles)
)(Error)
