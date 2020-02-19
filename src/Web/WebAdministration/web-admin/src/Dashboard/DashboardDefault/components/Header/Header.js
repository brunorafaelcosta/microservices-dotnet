import React from 'react'
import PropTypes from 'prop-types'
import { compose } from 'recompose'
import clsx from 'clsx'
import { withStyles } from '@material-ui/styles'
import { Typography } from '@material-ui/core'

const styles = theme => ({
  root: {}
})

class Header extends React.Component {
  constructor(props) {
    super(props)
  }

  render() {
    const { classes, className, ...rest } = this.props

    return (
      <div
        {...rest}
        className={clsx(classes.root, className)}
      >
        <Typography
          component="h2"
          gutterBottom
          variant="overline"
        >
          Dashboards blablabla 
        </Typography>
      </div>
    )
  }
}

Header.propTypes = {
  className: PropTypes.string
}
Header.defaultProps = {
}

export default compose(
  withStyles(styles)
)(Header)
