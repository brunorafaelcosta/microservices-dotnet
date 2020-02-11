/* eslint-disable no-unused-vars */
import React, { useState, useRef, useEffect } from 'react';
import { Link as RouterLink } from 'react-router-dom';
import clsx from 'clsx';
import PropTypes from 'prop-types';
import { useDispatch } from 'react-redux';
import { makeStyles } from '@material-ui/styles';
import {
  AppBar,
  Button,
  IconButton,
  Toolbar,
  Hidden,
} from '@material-ui/core';
import InputIcon from '@material-ui/icons/Input';
import MenuIcon from '@material-ui/icons/Menu';

import { useTranslation } from 'react-i18next'
import useRouter from '../../../../../__web-common/utils/useRouter';
import { AppContext } from '../../../../../__web-common/context';
// import { logout } from '../../../../actions';

const useStyles = makeStyles(theme => ({
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
}));

const TopBar = props => {
  const { onOpenNavBarMobile, className, ...rest } = props;

  const classes = useStyles();
  const { history } = useRouter();
  const { t } = useTranslation();

  useEffect(() => {
    let mounted = true;

    return () => {
      mounted = false;
    };
  }, []);

  const handleLogout = () => {
    // history.push('/auth/login');
    // dispatch(logout());
  };

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
  );
};

TopBar.propTypes = {
  className: PropTypes.string,
  onOpenNavBarMobile: PropTypes.func
};

export default TopBar;
