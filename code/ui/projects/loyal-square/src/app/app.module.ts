import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NgxLibModule } from 'ngx-lib';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import jss from 'jss';
import jssPresetDefault from 'jss-preset-default';
import jssDynamic from 'jss-plugin-rule-value-function';
import { FooterComponent } from './components/footer/footer.component';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { CareersComponent } from './pages/careers/careers.component';
import { AboutComponent } from './pages/about/about.component';
import { HelpPageComponent } from './pages/help-page/help-page.component';
import { AnnouncementsComponent } from './pages/announcements/announcements.component';
import { VerifyAccountComponent } from './pages/verify-account/verify-account.component';
import { ShellComponent } from './components/shell/shell.component';
import { HomeComponent } from './components/home/home.component';
jss.setup(jssPresetDefault());
jss.use(jssDynamic());

@NgModule({
  declarations: [
    AppComponent,
    FooterComponent,
    LoginComponent,
    RegisterComponent,
    CareersComponent,
    AboutComponent,
    HelpPageComponent,
    AnnouncementsComponent,
    VerifyAccountComponent,
    ShellComponent,
    HomeComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NgxLibModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
