import { Component, OnInit } from '@angular/core';
import { ThemeService } from './services/themes/theme.service';
import jss, { Classes } from 'jss';
import { ThemeStructure } from './services/themes/theme.types';
import { AuthService } from './services/auth.service';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
})
export class AppComponent implements OnInit {
  title = 'loyalSquare';
  classes: Classes<keyof ThemeStructure> | undefined;
  constructor(
    private themeService: ThemeService,
    private authService: AuthService
  ) {}
  public async ngOnInit(): Promise<void> {
    await this.authService.init();
    await this.themeService.init();
    this.extractClassesFromThemeService();
  }
  private extractClassesFromThemeService() {
    this.classes = this.themeService.themeClasses;
  }
  public toggleTheme() {
    this.themeService.toggleLightDarkTheme();
    this.extractClassesFromThemeService();
    console.log(this.classes);
  }
  public overrideTheme() {
    this.themeService.overrideLoyalSquareTheme();
  }
  public logout() {
    this.authService.signOut();
  }
}
