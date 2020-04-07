import {
    Directive,
    Input,
    OnDestroy,
    OnInit,
    TemplateRef,
    ViewContainerRef
} from '@angular/core';
import { Subject } from 'rxjs';

import { AuthenticationService } from '../authentication';

@Directive({
    selector: '[userHasPermission]'
})
export class HasPermissionDirective implements OnInit, OnDestroy {
    @Input() userHasPermission: string;

    stop$ = new Subject();

    isVisible = false;

    constructor(
        private viewContainerRef: ViewContainerRef,
        private templateRef: TemplateRef<any>,
        private authenticationService: AuthenticationService
    ) { }

    ngOnInit() {
        if (!this.authenticationService.IsAuthorized) {
            this.viewContainerRef.clear();
        }

        if (this.authenticationService.HasPermission(this.userHasPermission)) {
            if (!this.isVisible) {
                this.isVisible = true;
                this.viewContainerRef.createEmbeddedView(this.templateRef);
            }
        } else {
            this.isVisible = false;
            this.viewContainerRef.clear();
        }
    }
    
    ngOnDestroy() {
        this.stop$.next();
    }
}