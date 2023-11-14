import { Injectable, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { ThemeColors, ThemeStructure } from './theme.types';
import jss, { Classes, StyleSheet } from 'jss';

@Injectable({
  providedIn: 'root',
})
export class ThemeService {
  public currentTheme: ThemeColors | undefined;
  public themeClasses: Classes<keyof ThemeStructure> | undefined;
  public loyalsquareStyles: ThemeStructure | undefined;
  public sheet: StyleSheet<keyof ThemeStructure> | undefined;

  public init(): void {
    this.currentTheme = JSON.parse(
      localStorage.getItem('theme') ?? JSON.stringify(loyalsquareDark)
    );
    this.changeColorTheme();
    if (this.loyalsquareStyles) {
      this.sheet = jss
        .createStyleSheet(this.loyalsquareStyles, { link: true })
        .attach()
        .update(this.currentTheme ?? {});
      this.themeClasses = this.sheet.classes;
    }
  }

  public changeColorTheme() {
    this.loyalsquareStyles = {
      heading: {
        color: this.currentTheme?.colors.secondary1 ?? 'black',
        fontSize: this.currentTheme?.sizes?.headingSize ?? '2rem',
        fontWeight: 700,
        fontFamily: 'Arial, sans-serif',
      },
      subHeading: {
        color: this.currentTheme?.colors.secondary1 ?? 'black',
        fontSize: this.currentTheme?.sizes?.headingSize ?? '1.3rem',
        fontWeight: 200,
        fontFamily: 'Arial, sans-serif',
      },
      base: {
        backgroundColor: (data) => data.colors.primary1 ?? 'white',
        color: (data) => data.colors.secondary1 ?? 'black',
        height: (data) => '100vh',
        display: (data) => 'block',
      },
    };
    if (this.sheet) {
      this.sheet.update(this.currentTheme ?? {});
      this.themeClasses = this.sheet.classes;
    }
  }

  public toggleLightDarkTheme() {
    if (this.currentTheme?.name === 'loyalsquare-light') {
      this.currentTheme = loyalsquareDark;
    } else {
      this.currentTheme = loyalsquareLight;
    }

    this.changeColorTheme();
  }

  public overrideLoyalSquareTheme(customTheme: ThemeColors) {
    this.currentTheme = customTheme;
    this.sheet?.update(this.currentTheme);
  }
}

export const loyalsquareLight: ThemeColors = {
  name: 'loyalsquare-light',
  colors: {
    primary1: 'white',
    secondary1: 'black',
    tertiary1: '#4387FF',
  },
};

export const loyalsquareDark: ThemeColors = {
  name: 'loyalsquare-dark',
  colors: {
    primary1: 'black',
    secondary1: 'white',
    tertiary1: '#4387FF',
  },
};
