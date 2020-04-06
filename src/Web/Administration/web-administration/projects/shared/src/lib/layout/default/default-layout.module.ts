import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';

import { DefaultLayoutComponent } from './default-layout.component';
import { NavbarComponent } from './components';

@NgModule({
  declarations: [
    DefaultLayoutComponent,
    NavbarComponent,
  ],
  imports: [
    BrowserModule,
    RouterModule,
  ],
  exports: [
    DefaultLayoutComponent,
    NavbarComponent,
  ],
})
export class DefaultLayoutModule {
}
