import { Injectable, Inject } from '@angular/core';
import { APP_BASE_HREF } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';

import { LoggerService } from './logger.service';
import { CoreConfig } from '../core.config';
import { UrlUtils } from '../utils';

@Injectable({
    providedIn: 'root',
})
export class RouterService {
    constructor(
        private _config: CoreConfig,
        @Inject(APP_BASE_HREF) private _baseHref: string,
        private _router: Router,
        private _route: ActivatedRoute,
        private _logger: LoggerService,
    ) {
    }

    public redirect(path: string): Promise<boolean> {
        this._logger.Debug('Redirecting to "' + path + '"');

        return this._router.navigate([path]);
    }

    public redirectToDefaultRoute(): Promise<boolean> {
        const path: string = this._config.DefaultRoutePath;

        return this.redirect(path);
    }

    public GetDefaultUrl(): string {
        return UrlUtils.MergeUrlPaths(
            location.origin,
            this._baseHref,
            this._config.DefaultRoutePath,
        );
    }
}
