import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root',
})
export class LocalizationService {
    private defaultCulture: string;

    constructor() {
        this.defaultCulture = 'en';
    }

    init() : void {
        console.log('LocalizationService init...');
    }
}
