import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';

import { LayoutService } from './layout.service';
import { LayoutComponent } from './layout.component';
import { DefaultLayoutModule } from './default';
import { ErrorLayoutComponent } from './error';

@NgModule({
  declarations: [
    LayoutComponent,
    ErrorLayoutComponent,
  ],
  imports: [
    BrowserModule,
    RouterModule,

    DefaultLayoutModule,
  ],
  exports: [
    LayoutComponent,
    ErrorLayoutComponent,
  ],
  providers: [
    LayoutService,
  ],
})
export class LayoutModule {
}
