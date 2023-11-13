import { Component, OnInit } from '@angular/core';
import { ThemeService } from './services/themes/theme.service';
import jss, { Classes } from 'jss';
import { ThemeStructure } from './services/themes/theme.types';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
})
export class AppComponent implements OnInit {
  title = 'loyalSquare';
  classes: Classes<keyof ThemeStructure> | undefined;
  constructor(private themeService: ThemeService){}
  public ngOnInit(): void {
    this.themeService.init();
    this.extractClassesFromThemeService();
  }
  private extractClassesFromThemeService(){
    this.classes = this.themeService.themeClasses;
  }
  public toggleTheme(){
    this.themeService.toggleLightDarkTheme();
    this.extractClassesFromThemeService();
    console.log(this.classes);
    
  }
}
