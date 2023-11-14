export type ThemeColors = {
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
    background: (data: ThemeColors) => string;
    color: (data: ThemeColors) => string;
    height: (data: ThemeColors) => string;
    display: (data: ThemeColors) => string;
  };
  baseColors: {
    background: (data: ThemeColors) => string;
    color: (data: ThemeColors) => string;
  };
  baseOverlay: {
    background: (data: ThemeColors) => string;
  },
  baseOverlayInverted: {
    background: (data: ThemeColors) => string;
  },
  baseColorsInverted: {
    background: (data: ThemeColors) => string;
    color: (data: ThemeColors) => string;
  };
  baseBackgroundTertiary: {
    background: (data: ThemeColors) => string;
  };
  linkStyles: {
    color: (data: ThemeColors) => string;
    '&:hover': {
      color: (data: ThemeColors) => string;
    };
  };
  linkStylesInverted: {
    color: (data: ThemeColors) => string;
    '&:hover': {
      color: (data: ThemeColors) => string;
    };
  };
};
