import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root',
})
export class LoggerService {
    constructor() {
    }

    public Debug(message: any): void {
        console.log('Debug: ', message);
    }

    public Warning(message: any): void {
        console.log('Warning: ', message);
    }

    public Exception(message: any): void {
        console.log('Exception: ', message);
    }
}
