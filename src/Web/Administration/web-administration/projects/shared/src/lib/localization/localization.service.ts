import { Injectable } from '@angular/core';

import { LoggerService } from '../helpers';
import { CoreConfig } from '../core.config';

@Injectable({
    providedIn: 'root',
})
export class LocalizationService {
    private defaultCulture: string;

    constructor(
        private config: CoreConfig,
        private logger: LoggerService,
    ) {
        this.defaultCulture = 'en';
    }

    init() : void {
        this.logger.Debug('LocalizationService init...');
    }
}
