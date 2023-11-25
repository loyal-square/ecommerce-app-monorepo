import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { CareersComponent } from './pages/careers/careers.component';
import { AboutComponent } from './pages/about/about.component';
import { HelpPageComponent } from './pages/help-page/help-page.component';
import { AnnouncementsComponent } from './pages/announcements/announcements.component';
import { VerifyAccountComponent } from './pages/verify-account/verify-account.component';
import { ShellComponent } from './components/shell/shell.component';
import { HomeComponent } from './components/home/home.component';

const routes: Routes = [
  {
    path: '',
    component: ShellComponent,
    children: [
      {
        path: '',
        component: HomeComponent,
      },
      {
        path: 'careers',
        component: CareersComponent,
      },
      {
        path: 'about',
        component: AboutComponent,
      },
      {
        path: 'help',
        component: HelpPageComponent,
      },
      {
        path: 'announcements',
        component: AnnouncementsComponent,
      },
    ],
  },
  {
    path: 'login',
    component: LoginComponent,
  },
  {
    path: 'register',
    component: RegisterComponent,
  },
  {
    path: 'verify-account',
    component: VerifyAccountComponent,
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
