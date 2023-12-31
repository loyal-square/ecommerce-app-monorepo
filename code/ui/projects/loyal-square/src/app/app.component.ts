import { Component, OnInit } from '@angular/core';
import { ThemeService } from './services/themes/theme.service';
import { Classes } from 'jss';
import { ThemeStructure } from './services/themes/theme.types';
import { AuthService } from './services/auth.service';
import { IconService } from './services/icon.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
})
export class AppComponent implements OnInit {
  title = 'loyalSquare';
  classes: Classes<keyof ThemeStructure> | undefined;
  initializing: boolean = false;

  constructor(
    private themeService: ThemeService,
    private authService: AuthService,
    private iconService: IconService
  ) {}

  public async ngOnInit(): Promise<void> {
    this.initializing = true;
    await this.authService.init();
    this.themeService.init();
    this.iconService.init();
    this.extractClassesFromThemeService();
    this.initializing = false;
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
