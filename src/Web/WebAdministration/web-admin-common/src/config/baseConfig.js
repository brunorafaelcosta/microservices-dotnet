const baseConfig = {
    defaultLanguage: 'en',

    defaultTheme: {
        primary: '#0a6e24',
        secondary: '#004011'
    },

    webAuthenticationUrl: 'https://login.sapo.pt/',

    router: {
        onNotFoundRouteRedirectToPath: '/errors/error-404',
        onUnauthorizedRouteRedirectToPath: '/errors/error-404'
    }
};

export default baseConfig;