import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root',
})
export class AuthenticationService {
    constructor() {
    }

    init() : void {
        console.log('AuthenticationService init...');
    }
}
