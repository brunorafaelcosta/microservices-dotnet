import React from 'react'
import { compose } from 'recompose'
import { withStyles } from '@material-ui/styles'
import { Grid } from '@material-ui/core'

import { Page, Label, ComponentAuthorizer } from 'web-common/src/components'
import { Header } from './components'

const styles = theme => ({
    root: {
        padding: theme.spacing(3)
    },
    container: {
        marginTop: theme.spacing(3)
    }
})

class DashboardDefault extends React.Component {
    constructor(props) {
        super(props)
    }

    render() {
        const { classes, ...rest } = this.props

        return (
            <Page
                className={classes.root}
                title="Default Dashboard"
            >
                <div> texto asdasdasd </div>
                <Header />
                <Grid
                    className={classes.container}
                    container
                    spacing={3}
                >
                    <Grid
                        item
                        lg={3}
                        sm={6}
                        xs={12}
                    >
                        Charts can go here...
                    <br /><br />
                        <ComponentAuthorizer Permissions='defaultdashlabel'>
                            <Label>Private content goes here</Label>
                        </ComponentAuthorizer>
                    </Grid>
                </Grid>
            </Page>
        )
    }
};

export default compose(
    withStyles(styles)
)(DashboardDefault)
