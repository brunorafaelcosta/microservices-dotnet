import { Injectable } from '@angular/core';
import {
    CanActivate,
    ActivatedRouteSnapshot,
    RouterStateSnapshot,
    Router,
    Route
} from '@angular/router';
import { Observable } from 'rxjs';

import { CoreConfig } from '../core.config';
import { AuthenticationService } from '../authentication';
import { RouterService } from '../helpers';

@Injectable()
export class AuthenticationGuard implements CanActivate {
    constructor(
        private config: CoreConfig,
        private authenticationService: AuthenticationService,
        private routerService: RouterService,
        private router: Router) {
    }

    canActivate(
        next: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): Observable<boolean> | Promise<boolean> | boolean {
        if (this.authenticationService.IsAuthorized) {
            return true;
        }

        return this.routerService.redirect(this.config.OnUnauthorizedRouteRedirectToPath);
    }
}