import { Injectable } from '@angular/core';

import { LoggerService } from '../helpers';

@Injectable({
    providedIn: 'root',
})
export class ThemeService {
    constructor(
        private logger: LoggerService,
    ) {
    }

    init() : void {
        this.logger.Debug('ThemeService init...');
    }
}
