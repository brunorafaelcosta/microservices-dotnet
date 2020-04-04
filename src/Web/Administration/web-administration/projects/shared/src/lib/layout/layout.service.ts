import { Injectable } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { BehaviorSubject } from 'rxjs';

import { LoggerService } from '../helpers';
import { CoreConfig } from '../core.config';

@Injectable({
    providedIn: 'root',
})
export class LayoutService {
    private title: BehaviorSubject<string>;

    constructor (
        private config: CoreConfig,
        private titleService: Title,
        private logger: LoggerService,
    ) {
    }

    init() : void {
        this.logger.Debug('LayoutService init...');

        this.title = new BehaviorSubject<string>(this.config.AppTitle);
        this.title.asObservable().subscribe(title => {
            this.titleService.setTitle(title);
        });
    }

    setTitle(title: string) {
        const nextTitle = this.getFullTitle(title, this.title.getValue());
        this.title.next(nextTitle);
    }

    private getFullTitle(newTitle: string, currentTitle: string): string {
        let fullTitle = currentTitle || this.config.AppTitle || '';
        
        if (newTitle) {
            fullTitle = (fullTitle !== '')
                ? newTitle + ' - ' + fullTitle
                : newTitle;
        }

        return fullTitle;
    }
}
