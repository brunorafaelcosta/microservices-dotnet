import { Injector } from '@angular/core';
import { PlatformLocation } from '@angular/common';

import { DocumentUtils } from './utils';
import { CoreBootstrap } from './coreBootstrap';

export function CoreInitializerFactory(
    injector: Injector,
    platformLocation: PlatformLocation) {
    return () => {
        return new Promise<boolean>((resolve, reject) => {
            let appBaseUrl = DocumentUtils.GetDocumentOrigin();

            CoreBootstrap.run(injector, appBaseUrl, () => {
                setTimeout(function () {
                    resolve(true);
                }, 500)


            }, resolve, reject);
        });
    };
};
