import { Component, OnInit } from '@angular/core';
import { ThemeService } from '../../services/themes/theme.service';
import { Classes } from 'jss';
import { ThemeStructure } from '../../services/themes/theme.types';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css'],
})
export class FooterComponent implements OnInit {
  public classes: Classes<keyof ThemeStructure> | undefined;

  constructor(public themeService: ThemeService) {}
  public ngOnInit(): void {
    this.classes = this.themeService.themeClasses;
  }

  public getImageLogoUrl(): string {
    return this.themeService.currentTheme?.name.includes('light')
      ? '../../../assets/images/logoLight.png'
      : '../../../assets/images/logoDark.png';
  }
}
