import React from 'react';
import { makeStyles } from '@material-ui/styles';
import { Grid } from '@material-ui/core';

import { Page, Label } from '../../__web-common/components';
import { ComponentAuthorizer } from '../../__web-common/components';

import { Header } from './components';
console.log('private code xD');
const useStyles = makeStyles(theme => ({
    root: {
        padding: theme.spacing(3)
    },
    container: {
        marginTop: theme.spacing(3)
    }
}));

const DashboardDefault = () => {
    const classes = useStyles();

    return (
        <Page
            className={classes.root}
            title="Default Dashboard"
        >
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
                    Charts can go here
                    <br /><br />
                    <ComponentAuthorizer Permissions='defaultdashlabel'>
                        <Label>Private content goes here</Label>
                    </ComponentAuthorizer>
                </Grid>
            </Grid>
        </Page>
    );
};

export default DashboardDefault;