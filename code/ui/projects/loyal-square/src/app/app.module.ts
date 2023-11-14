import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NgxLibModule } from 'ngx-lib';

import jss from 'jss';
import jssPresetDefault from 'jss-preset-default';
import jssDynamic from 'jss-plugin-rule-value-function';
jss.setup(jssPresetDefault());
jss.use(jssDynamic());

@NgModule({
  declarations: [AppComponent],
  imports: [BrowserModule, AppRoutingModule, NgxLibModule],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
