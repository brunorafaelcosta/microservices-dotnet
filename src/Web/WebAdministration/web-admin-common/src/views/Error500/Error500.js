import React from 'react'
import { Link as RouterLink } from 'react-router-dom'
import { compose } from 'recompose'
import { withStyles } from '@material-ui/styles'
import { Typography, Button, withTheme } from '@material-ui/core'

import { Page } from 'web-common/src/components';

const styles = theme => ({
  root: {
    padding: theme.spacing(3),
    paddingTop: '10vh',
    display: 'flex',
    flexDirection: 'column',
    alignContent: 'center'
  },
  imageContainer: {
    marginTop: theme.spacing(6),
    display: 'flex',
    justifyContent: 'center'
  },
  image: {
    maxWidth: '100%',
    width: 560,
    maxHeight: 300,
    height: 'auto'
  },
  buttonContainer: {
    marginTop: theme.spacing(6),
    display: 'flex',
    justifyContent: 'center'
  }
})

class Error500 extends React.Component {
  constructor(props) {
    super(props)
  }

  render() {
    const { theme, classes, ...rest } = this.props

    return (
      <Page
        className={classes.root}
        title="Error 500"
      >
        <Typography
          align="center"
        >
          500: Ooops, something went terribly wrong!
        </Typography>
        <Typography
          align="center"
          variant="subtitle2"
        >
          You either tried some shady route or you came here by mistake. Whichever
          it is, try using the navigation
        </Typography>
        <div className={classes.imageContainer}>
          <img
            alt="Under development"
            className={classes.image}
            src="/images/undraw_server_down_s4lk.svg"
          />
        </div>
        <div className={classes.buttonContainer}>
          <Button
            color="primary"
            component={RouterLink}
            to="/"
            variant="outlined"
          >
            Back to home
          </Button>
        </div>
      </Page>
    )
  }
}

export default compose(
  withStyles(styles),
  withTheme
)(Error500)
