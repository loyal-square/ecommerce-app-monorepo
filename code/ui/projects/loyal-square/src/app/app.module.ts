import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NgxLibModule } from 'ngx-lib';

import jss from 'jss';
import jssPresetDefault from 'jss-preset-default';
import jssDynamic from 'jss-plugin-rule-value-function';
import { FooterComponent } from "./components/footer/footer.component";
import { MatIconModule } from '@angular/material/icon';
import { HttpClientModule } from '@angular/common/http';
import { SvgVerifyComponent } from './components/svgs/svg-verify/svg-verify.component';
import { SvgLoginComponent } from './components/svgs/svg-login/svg-login.component';
import { SvgRegisterComponent } from './components/svgs/svg-register/svg-register.component';
import { SvgSignupComponent } from './components/svgs/svg-signup/svg-signup.component';
import { SvgAccountDetailsComponent } from './components/svgs/svg-account-details/svg-account-details.component';
import { SvgStoreAccountComponent } from './components/svgs/svg-store-account/svg-store-account.component';

jss.setup(jssPresetDefault());
jss.use(jssDynamic());

@NgModule({
  declarations: [AppComponent, FooterComponent, SvgVerifyComponent, SvgLoginComponent, SvgRegisterComponent, SvgSignupComponent, SvgAccountDetailsComponent, SvgStoreAccountComponent],
  imports: [BrowserModule, AppRoutingModule, NgxLibModule, MatIconModule, HttpClientModule],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule { }
