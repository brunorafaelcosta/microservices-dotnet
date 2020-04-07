import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

import { LoggerService, StorageService, RouterService } from '../helpers';
import { UrlUtils } from '../utils';
import { CoreConfig } from '../core.config';
import { AuthenticationConfig } from './authentication.config';
import { IIdentity } from './identity.model';

@Injectable({
    providedIn: 'root',
})
export class AuthenticationService {
    constructor(
        private config: AuthenticationConfig,
        private coreConfig: CoreConfig,
        private http: HttpClient,
        private router: RouterService,
        private storage: StorageService,
        private logger: LoggerService,
    ) {
        this.IsAuthorized = false;
        this.User = null;
    }

    Init() : void {
        if (typeof this.storage.retrieve('IsAuthorized') === 'boolean') {
            this.IsAuthorized = this.storage.retrieve('IsAuthorized');
            this.User = this.storage.retrieve('userInfo') || null;
        }

        if (window.location.hash) {
            this.loginCallback();
        } else if (!this.IsAuthorized) {
            this.Login();
        }
    }

    public IsAuthorized: boolean;
    public User: IIdentity;

    public Login() {
        this.resetAuthorizationData();

        const loginUrl = this.config.AuthorityLoginUrl;
        const clientId = this.config.AuthorityClientId;
        const scope = this.config.AuthorityScope;
        const responseType = 'id_token token';
        const redirectUri = this.config.AuthorityRedirectUrl;
        const nonce = 'N' + Math.random() + '' + Date.now();
        const state = Date.now() + '' + Math.random();

        this.storage.store('authStateControl', state);
        this.storage.store('authNonce', nonce);

        let url =
            loginUrl + '?' +
            'client_id=' + encodeURI(clientId) + '&' +
            'scope=' + encodeURI(scope) + '&' +
            'response_type=' + encodeURI(responseType) + '&' +
            'redirect_uri=' + encodeURI(redirectUri) + '&' +
            'nonce=' + encodeURI(nonce) + '&' +
            'state=' + encodeURI(state);

        window.location.href = url;
    }

    public Logout() {
        const logoutUrl = this.config.AuthorityLogoutUrl;
        const redirectUri = this.config.AuthorityRedirectUrl;
        const idTokenHint = this.storage.retrieve('authorizationDataIdToken');

        let url =
            logoutUrl + '?' +
            'id_token_hint=' + encodeURI(idTokenHint) + '&' +
            'post_logout_redirect_uri=' + encodeURI(redirectUri);

        this.resetAuthorizationData();

        window.location.href = url;
    }

    public HasPermissions(permissions: string[], matchAll: boolean = true): boolean {
        let has: boolean = true;

        if (permissions) {
            if (matchAll) {
                has = 
                    this.User &&
                    this.User.Permissions &&
                    permissions.every(p=> this.User.Permissions.indexOf(p) > -1);
            } else {
                has = 
                    this.User &&
                    this.User.Permissions &&
                    permissions.some(p=> this.User.Permissions.indexOf(p) > -1);
            }
        }

        return has;
    }

    public HasPermission(permission: string): boolean {
        let has: boolean = true;

        if (permission) {
            has = 
                this.User &&
                this.User.Permissions &&
                this.User.Permissions.indexOf(permission) > -1;
        }

        return has;
    }

    private loginCallback() {
        this.resetAuthorizationData();

        let hash = window.location.hash.substr(1);
        let result: any = hash.split('&').reduce(function (result: any, item: string) {
            let parts = item.split('=');
            result[parts[0]] = parts[1];
            return result;
        }, {});

        let token = '';
        let idToken = '';
        let authResponseIsValid = false;

        if (!result.error) {
            if (result.state !== this.storage.retrieve('authStateControl')) {
                this.logger.Warning('AuthorizedCallback incorrect state');
            } else {
                token = result.access_token;
                idToken = result.id_token;

                let idTokenData: any = {};
                if (typeof idToken !== 'undefined') {
                    let idTokenEncoded = idToken.split('.')[1];
                    idTokenData = JSON.parse(UrlUtils.UrlBase64Decode(idTokenEncoded));
                }

                // validate nonce
                if (idTokenData.nonce !== this.storage.retrieve('authNonce')) {
                    this.logger.Warning('AuthorizedCallback incorrect nonce');
                } else {
                    this.storage.store('authNonce', '');
                    this.storage.store('authStateControl', '');

                    authResponseIsValid = true;
                    this.logger.Debug('AuthorizedCallback state and nonce validated, returning access token');
                }
            }
        }

        if (authResponseIsValid) {
            this.setAuthorizationData(token, idToken);
        } else {
            this.router.redirect(this.coreConfig.OnUnauthorizedRouteRedirectToPath);
        }
    }

    private setAuthorizationData(token: any, idToken: any) {
        if (this.storage.retrieve('authorizationData') !== '') {
            this.storage.store('authorizationData', '');
        }

        this.storage.store('authorizationData', token);
        this.storage.store('authorizationDataIdToken', idToken);
        this.storage.store('isAuthorized', true);
        this.IsAuthorized = true;

        this.getUserInfo()
            .subscribe(info => {
                this.User = {
                    PreferredUsername: info['preferred_username'],
                    Name: info['name'],
                    LoginDate: new Date(),
                    Permissions: this.getParsedPermissions(info['permissions']),
                };
                this.storage.store('userInfo', this.User);
                
                this.router.redirectToDefaultRoute();
            },
            error => {
                this.logger.Warning(error);

                if (error.status == 403) {
                    this.router.redirect(this.coreConfig.OnForbiddenRouteRedirectToPath);
                }
                else if (error.status == 401) {
                    this.router.redirect(this.coreConfig.OnUnauthorizedRouteRedirectToPath);
                }
            },
            () => {
                this.logger.Debug('Logged user info: ', this.User);
            });
    }
    private resetAuthorizationData() {
        this.storage.store('authorizationData', '');
        this.storage.store('authorizationDataIdToken', '');

        this.IsAuthorized = false;
        this.User = null;
        this.storage.store('isAuthorized', false);
        this.storage.store('userInfo', '');
    }

    private getToken(): any {
        return this.storage.retrieve('authorizationData');
    }

    private getUserInfo = (): Observable<string[]> => {
        const httpUrl = this.config.AuthorityUserInfoUrl;
        
        const httpOptions = {
            headers: new HttpHeaders()
        };
        httpOptions.headers = httpOptions.headers.set('Content-Type', 'application/json');
        httpOptions.headers = httpOptions.headers.set('Accept', 'application/json');

        const token = this.getToken();
        if (token !== '') {
            httpOptions.headers = httpOptions.headers.set('Authorization', `Bearer ${token}`);
        }

        return this.http
            .get<string[]>(httpUrl, httpOptions)
            .pipe<string[]>((info: any) => info);
    }

    private getParsedPermissions(permissions: string): string[] {
        let permissionsArray: string[] = [];

        if (permissions) {
            permissions = permissions.replace(/\s+/g, '');

            permissionsArray = permissions.split(';');
        }
        
        return permissionsArray;
    }
}
