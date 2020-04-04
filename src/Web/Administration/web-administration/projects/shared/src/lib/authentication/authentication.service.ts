import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders }      from '@angular/common/http';
import { Observable } from 'rxjs';

import { LoggerService, StorageService, RouterService } from '../helpers';
import { CoreConfig } from '../core.config';
import { AuthenticationConfig } from './authentication.config';
import { UrlUtils } from '../utils';

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
    }

    init() : void {
        if (typeof this.storage.retrieve('IsAuthorized') === 'boolean') {
            this.IsAuthorized = this.storage.retrieve('IsAuthorized');
            this.UserData = this.storage.retrieve('userData');
        }

        if (window.location.hash) {
            this.AuthorizedCallback();
        } else if (!this.IsAuthorized) {
            this.Authorize();
        }
    }

    public IsAuthorized: boolean;
    public UserData: any;

    public Authorize() {
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

    public AuthorizedCallback() {
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
        this.storage.store('IsAuthorized', true);
        this.IsAuthorized = true;

        this.getUserInfo()
            .subscribe(info => {
                this.UserData = info;
                this.storage.store('userData', info);
                
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
                this.logger.Debug(this.UserData);
            });
    }
    private resetAuthorizationData() {
        this.storage.store('authorizationData', '');
        this.storage.store('authorizationDataIdToken', '');

        this.IsAuthorized = false;
        this.storage.store('IsAuthorized', false);
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
}
