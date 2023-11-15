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
      localStorage.getItem('theme') ?? JSON.stringify(loyalsquareLight)
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
        background: (data) => data.colors.primary1 ?? 'white',
        color: (data) => data.colors.secondary1 ?? 'black',
        height: (data) => '100vh',
        display: (data) => 'block',
      },
      baseColors: {
        background: (data) => data.colors.primary1 ?? 'white',
        color: (data) => data.colors.secondary1 ?? 'black',
      },
      baseOverlay: {
        background: (data) =>
          data.colors.primary1Transparent ?? 'rgba(255,255,255,0.9)',
      },
      baseOverlayInverted: {
        background: (data) =>
          data.colors.secondary1Transparent ?? 'rgba(0,0,0,0.9)',
      },
      baseBackgroundTertiary: {
        background: (data) => data.colors.tertiary1 ?? 'white',
      },
      baseColorsInverted: {
        background: (data) => data.colors.secondary1 ?? 'black',
        color: (data) => data.colors.primary1 ?? 'white',
      },
      linkStyles: {
        color: (data) => data.colors.secondary1 ?? 'black',
        '&:hover': {
          color: (data) => data.colors.tertiary1 ?? 'blue',
        },
      },
      linkStylesInverted: {
        color: (data) => data.colors.primary1 ?? 'white',
        '&:hover': {
          color: (data) => data.colors.tertiary1 ?? 'blue',
        },
      },
    };
    if (this.sheet) {
      this.sheet.update(this.currentTheme ?? {});
      this.themeClasses = this.sheet.classes;
    }
  }

  public toggleLightDarkTheme() {
    if (this.currentTheme?.name.includes('loyalsquare')) {
      if (this.currentTheme?.name === 'loyalsquare-light') {
        this.currentTheme = loyalsquareDark;
      } else {
        this.currentTheme = loyalsquareLight;
      }
    } else {
      if (this.currentTheme?.name === 'override-light') {
        this.currentTheme = overrideDark;
      } else {
        this.currentTheme = overrideLight;
      }
    }

    this.changeColorTheme();
  }

  public overrideLoyalSquareTheme(uniqueColor?: string | undefined) {
    if (uniqueColor) {
      overrideDark.colors.tertiary1 = uniqueColor;
      overrideLight.colors.tertiary1 = uniqueColor;
    }
    this.currentTheme = overrideLight;
    this.sheet?.update(this.currentTheme);
  }
}
export let overrideLight: ThemeColors = {
  name: 'override-light',
  colors: {
    primary1: 'white',
    secondary1: 'black',
    tertiary1: 'pink',
  },
};

export let overrideDark: ThemeColors = {
  name: 'override-dark',
  colors: {
    primary1: 'black',
    secondary1: 'white',
    tertiary1: 'pink',
  },
};

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
