import { Component, OnInit } from '@angular/core';
import { ThemeService } from './services/themes/theme.service';
import { Classes } from 'jss';
import { ThemeStructure } from './services/themes/theme.types';
import { IconService } from './services/icon.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
})
export class AppComponent implements OnInit {
  title = 'loyalSquare';
  classes: Classes<keyof ThemeStructure> | undefined;

  constructor(private themeService: ThemeService, private iconService: IconService) { }

  public ngOnInit(): void {
    this.iconService.init();
    this.themeService.init();

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
}
