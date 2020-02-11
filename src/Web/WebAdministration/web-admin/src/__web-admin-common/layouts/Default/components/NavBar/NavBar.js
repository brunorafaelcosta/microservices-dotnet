/* eslint-disable no-unused-vars */
import React, { Fragment, useEffect } from 'react';
import clsx from 'clsx';
import PropTypes from 'prop-types';
// import { useSelector } from 'react-redux';
import { makeStyles } from '@material-ui/styles';
import { Drawer, Divider, Paper, Typography, Hidden } from '@material-ui/core';
import PerfectScrollbar from "react-perfect-scrollbar";
import useRouter from '../../../../../__web-common/utils/useRouter';
import { useTranslation } from 'react-i18next'
import { ComponentAuthorizer } from '../../../../../__web-common/components';
import { Navigation } from '../../../../components';

const useStyles = makeStyles(theme => ({
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
}));

const NavBar = props => {
  const { navigationConfig, openMobile, onMobileClose, className, ...rest } = props;

  const classes = useStyles();
  const router = useRouter();
  // const session = useSelector(state => state.session);
  const { t } = useTranslation();

  useEffect(() => {
    if (openMobile) {
      onMobileClose && onMobileClose();
    }

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [router.location.pathname]);

  const navbarContent = (
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
  );

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
            {navbarContent}
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
          {navbarContent}
        </Paper>
      </Hidden>
    </Fragment>
  );
};

NavBar.propTypes = {
  className: PropTypes.string,
  onMobileClose: PropTypes.func,
  openMobile: PropTypes.bool,
  navigationConfig: PropTypes.array
};

export default NavBar;
