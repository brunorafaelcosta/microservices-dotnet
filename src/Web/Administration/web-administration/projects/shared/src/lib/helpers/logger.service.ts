import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root',
})
export class LoggerService {
    constructor() {
    }

    public Debug(message: string, ...optionalParams: any[]): void {
        this.log('Debug', message, ...optionalParams);
    }

    public Warning(message: string, ...optionalParams: any[]): void {
        this.log('Warning', message, ...optionalParams);
    }

    public Exception(message: string, ...optionalParams: any[]): void {
        this.log('Exception', message, ...optionalParams);
    }

    private log(type: string, message: string, ...optionalParams: any[]): void {
        optionalParams = optionalParams || [];

        if (optionalParams.length > 0) {
            console.log(`${type}: `, message, ...optionalParams);
        } else {
            console.log(`${type}: `, message);
        }
    }
}
