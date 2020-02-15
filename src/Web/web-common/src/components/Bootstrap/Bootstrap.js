import React, { Component } from 'react'
import { Router } from 'react-router-dom'
import { createBrowserHistory } from 'history'

import MomentUtils from '@date-io/moment';
import { ThemeProvider } from '@material-ui/styles';
import { MuiPickersUtilsProvider } from '@material-ui/pickers';
import { createMuiTheme } from '@material-ui/core/styles';

// import '../../mixins/chartjs';
// import '../../mixins/moment';
// import '../../mixins/validate';
// import '../../mixins/prismjs';

import InitialLoading from '../../layouts/Default';
import { CustomRouter, ScrollReset } from '../index'
import { RouterContext, AppContext } from '../../context'
import { authenticationService, localizationService, themeService } from '../../_services'

const history = createBrowserHistory();

export default class Bootstrap extends Component {
    constructor(props) {
        super(props)

        this.state = {
            isLoading: true,
            authentication: {
                loggedIn: false,
                user: {}
            },
            localization: {},
            theme: createMuiTheme({
                palette: {
                    primary: {
                        main: this.props.config.defaultTheme.primary
                    },
                    secondary: {
                        main: this.props.config.defaultTheme.secondary
                    }
                }
            })
        }

        this.onLogout = this.onLogout.bind(this);
    }

    componentDidMount() {
        const { config } = this.props;

        // Authentication
        authenticationService.getAuthenticatedUser()
            .then(currentUser => {
                if (currentUser)
                    this.setState({
                        authentication: {
                            loggedIn: true,
                            user: currentUser
                        }
                    })
                else
                    throw Error('Authentication failed!')

                return currentUser
            })

            // Localization
            .then((currentUser) => {
                return localizationService.initialize(config.moduleName, currentUser.tenantId, currentUser.culture)
                    .then(() => {
                        return currentUser
                    })
            })

            // Theme
            .then((currentUser) => {
                return themeService.getCurrentUserTheme()
                    .then(userTheme => {
                        let theme = {
                            palette: {
                                primary: {
                                    main: userTheme.data.theme.primary
                                },
                                secondary: {
                                    main: userTheme.data.theme.secondary
                                }
                            }
                        }

                        if (this.props.onUserThemeLoaded)
                            this.props.onUserThemeLoaded(theme)

                        this.setState({
                            theme: createMuiTheme(theme)
                        })

                        return currentUser
                    })
            })

            .then(() => {
                this.setState({
                    isLoading: false
                })

                if (this.props.onBootstrapCompleted)
                    this.props.onBootstrapCompleted()
            })

            .catch(function (error) {
                console.log('Bootstrap failed! Error:', error)
            })
    }

    onLogout() {
        authenticationService.logout()
            .then(() => {
                this.setState({
                    authentication: {
                        loggedIn: false,
                        user: {}
                    },
                    localization: {},
                    theme: {}
                });
            })
    }

    render() {
        const { isLoading, theme } = this.state
        const { routes } = this.props;

        const contextValue = {
            authentication: this.state.authentication,
            logoutUser: this.onLogout
        }

        return (
            <AppContext.Provider value={contextValue}>
                <ThemeProvider theme={theme}>
                    <MuiPickersUtilsProvider utils={MomentUtils}>
                        {!isLoading ? (
                            <Router history={history}>
                                <ScrollReset />
                                <CustomRouter routes={routes} />
                            </Router>
                        ) : (
                                <InitialLoading />
                            )}
                    </MuiPickersUtilsProvider>
                </ThemeProvider>
            </AppContext.Provider >
        );
    }
}