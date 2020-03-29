import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root',
})
export class ThemeService {
    constructor() {
    }

    init() : void {
        console.log('ThemeService init...');
    }
}
