export type ThemeValues = {
  name: string;
  colors: {
    primary1: string;
    primary2?: string;
    primary3?: string;
    secondary1: string;
    secondary2?: string;
    secondary3?: string;
    tertiary1: string;
    tertiary2?: string;
    tertiary3?: string;
    primary1Transparent?: string;
    primary2Transparent?: string;
    primary3Transparent?: string;
    secondary1Transparent?: string;
    secondary2Transparent?: string;
    secondary3Transparent?: string;
    tertiary1Transparent?: string;
    tertiary2Transparent?: string;
    tertiary3Transparent?: string;
  };
  sizes?: {
    headingSize?: string;
    subHeadingSize?: string;
    pSize?: string;
    pMiniSize?: string;
    fontWeight?: number;
    fancyBorderWidth: string
  };
};

export type ThemeStructure = {
  heading: {
    fontSize: string;
    fontFamily: string;
    color: string;
    fontWeight: number | string;
  };
  subHeading: {
    fontSize: string;
    fontFamily: string;
    color: string;
    fontWeight: number | string;
  };
  base: {
    background: (data: ThemeValues) => string;
    color: (data: ThemeValues) => string;
    height: (data: ThemeValues) => string;
    display: (data: ThemeValues) => string;
  };
  baseBorder: {
    border: (data: ThemeValues) => string;
  };
  baseColors: {
    background: (data: ThemeValues) => string;
    color: (data: ThemeValues) => string;
  };
  baseOverlay: {
    background: (data: ThemeValues) => string;
  },
  baseOverlayInverted: {
    background: (data: ThemeValues) => string;
  },
  baseColorsInverted: {
    background: (data: ThemeValues) => string;
    color: (data: ThemeValues) => string;
  };
  baseBackgroundTertiary: {
    background: (data: ThemeValues) => string;
  };
  linkStyles: {
    color: (data: ThemeValues) => string;
    '&:hover': {
      color: (data: ThemeValues) => string;
    };
  };
  linkStylesInverted: {
    color: (data: ThemeValues) => string;
    '&:hover': {
      color: (data: ThemeValues) => string;
    };
  };
};
