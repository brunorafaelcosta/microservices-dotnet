import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';

import { LayoutService } from './layout.service';
import { LayoutComponent } from './layout.component';
import { DefaultLayoutComponent } from './default';
import { ErrorLayoutComponent } from './error';

@NgModule({
  declarations: [
    LayoutComponent,
    DefaultLayoutComponent,
    ErrorLayoutComponent,
  ],
  imports: [
    BrowserModule,
    RouterModule,
  ],
  exports: [
    LayoutComponent,
    DefaultLayoutComponent,
    ErrorLayoutComponent,
  ],
  providers: [
    LayoutService,
  ],
})
export class LayoutModule {
}
