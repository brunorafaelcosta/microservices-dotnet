import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { CoreModule, LayoutComponent } from 'shared'

import RootConfig from './root.config';
const rootConfigInstance = new RootConfig();

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    BrowserModule,
    BrowserAnimationsModule,
    
    CoreModule.forRoot(rootConfigInstance),
  ],
  schemas: [
    CUSTOM_ELEMENTS_SCHEMA,
  ],
  bootstrap: [
    LayoutComponent,
  ],
})
export class RootModule {
}
