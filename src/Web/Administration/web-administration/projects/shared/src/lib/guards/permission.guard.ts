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
export class PermissionGuard implements CanActivate {
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
        if (!this.authenticationService.IsAuthorized) {
            return false;
        }
        
        const requiredPermissions: string[] = (next.data && next.data.permissions)
            ? (Array.isArray(next.data.permissions)
                ? next.data.permissions
                : [ next.data.permissions ])
            : [];
        
        const has = this.authenticationService.HasPermissions(requiredPermissions);
        
        if (!has) {
            return this.routerService.redirect(this.config.OnForbiddenRouteRedirectToPath);    
        }
        
        return true;
    }
}