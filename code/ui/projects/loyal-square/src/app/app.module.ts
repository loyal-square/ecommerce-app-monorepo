import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NgxLibModule } from 'ngx-lib';

import jss from 'jss';
import jssPresetDefault from 'jss-preset-default';
import jssDynamic from 'jss-plugin-rule-value-function';
import { FooterComponent } from "./components/footer/footer.component";
import { FancyHeaderComponent } from './components/fancy-header/fancy-header.component'
jss.setup(jssPresetDefault());
jss.use(jssDynamic());

@NgModule({
  declarations: [AppComponent, FooterComponent, FancyHeaderComponent],
  imports: [BrowserModule, AppRoutingModule, NgxLibModule],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
