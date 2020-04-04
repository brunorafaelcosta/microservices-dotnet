(function (window) {
    window['env'] = window['env'] || {};

    // Environment variables
    window['env']['authorityLoginUrl'] = '${AUTHORITYLOGINURL}';
    window['env']['authorityRedirectUrl'] = '${AUTHORITYREDIRECTURL}';
    window['env']['authorityLogoutUrl'] = '${AUTHORITYLOGOUTURL}';
    window['env']['authorityUserInfoUrl'] = '${AUTHORITYUSERINFOURL}';
})(this);
