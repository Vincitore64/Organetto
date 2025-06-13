import type { AntTheme } from './types'

const sharedTheme: AntTheme = {
  token: {
    fontFamily: 'Jost, Sofia Sans Extra Condensed, sans-serif',
    // colorTextLabel: 'var(--text-header)',
    // colorTextHeading: 'var(--text-header)',
    // colorTextBase: 'var(--text-primary)',
    // colorTextLightSolid: 'var(--text-primary)',
    // colorPrimaryHover: 'var(--primary-hover)',
    // // colorBgBase: 'var(--light-base-bg)',
    // // colorBgContainer: 'var(--light-base-bg)',
    // colorBgContainer: 'transparent',
    // boxShadow: 'var(--box-shadow-primary)',
    // boxShadowSecondary: 'var(--box-shadow-primary)',
    // colorBorder: 'var(--border-primary-color)',
  },
  components: {
    Card: {
      fontFamily: 'Sofia Sans Extra Condensed',
      fontSizeLG: 18,
    },
    // Spin: {
    //   colorPrimary: 'var(--icon-primary)',
    // },
    // Table: {
    //   fontSize: 12,
    // },
    // Button: {
    //   fontFamily: 'Jost, sans-serif',
    // },
    // Tabs: {
    //   fontFamily: 'Jost, sans-serif',
    //   fontSize: 16,
    //   colorBorderSecondary: 'transparent',
    //   colorBorder: 'transparent',
    // },
    // Divider: {
    //   marginLG: 10,
    // },
    // Input: {
    //   boxShadow: 'var(--box-shadow-primary)',
    // },
  },
}

export default sharedTheme
