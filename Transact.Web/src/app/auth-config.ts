/**
 * This file contains authentication parameters. Contents of this file
 * is roughly the same across other MSAL.js libraries. These parameters
 * are used to initialize Angular and MSAL Angular configurations in
 * in app.module.ts file.
 */

import {
  LogLevel,
  Configuration,
  BrowserCacheLocation,
} from '@azure/msal-browser';
import { environment } from 'src/environments/environment';

const isIE =
  window.navigator.userAgent.indexOf('MSIE ') > -1 ||
  window.navigator.userAgent.indexOf('Trident/') > -1;

/**
 * Enter here the user flows and custom policies for your B2C application,
 * To learn more about user flows, visit https://docs.microsoft.com/en-us/azure/active-directory-b2c/user-flow-overview
 * To learn more about custom policies, visit https://docs.microsoft.com/en-us/azure/active-directory-b2c/custom-policy-overview
 */
export const b2cPolicies = {
  names: {
    signIn: environment.signin_flow_name,
    signUp: environment.signup_flow_name,
  },
  authorities: {
    signIn: {
      authority: `https://${environment.azure_ad_b2c}.b2clogin.com/${environment.azure_ad_b2c}.onmicrosoft.com/B2C_1_${environment.signin_flow_name}`,
    },
    signUp: {
      authority: `https://${environment.azure_ad_b2c}.b2clogin.com/${environment.azure_ad_b2c}.onmicrosoft.com/B2C_1_${environment.signup_flow_name}`,
    },
  },
  authorityDomain: `${environment.azure_ad_b2c}.b2clogin.com`,
};
/**
 * Configuration object to be passed to MSAL instance on creation.
 * For a full list of MSAL.js configuration parameters, visit:
 * https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-browser/docs/configuration.md
 */
export const msalConfig: Configuration = {
  auth: {
    clientId: environment.web_clientId, // This is the ONLY mandatory field that you need to supply.
    authority: b2cPolicies.authorities.signIn.authority, // Defaults to "https://login.microsoftonline.com/common"
    knownAuthorities: [b2cPolicies.authorityDomain], // Mark your B2C tenant's domain as trusted.
    redirectUri: '/', // Points to window.location.origin. You must register this URI on Azure portal/App Registration.
    postLogoutRedirectUri: '/', // Indicates the page to navigate after logout.
    navigateToLoginRequestUrl: true, // If "true", will navigate back to the original request location before processing the auth code response.
  },
  cache: {
    cacheLocation: BrowserCacheLocation.LocalStorage, // Configures cache location. "sessionStorage" is more secure, but "localStorage" gives you SSO between tabs.
    storeAuthStateInCookie: isIE, // Set this to "true" if you are having issues on IE11 or Edge
  },
  system: {
    loggerOptions: {
      // loggerCallback(logLevel: LogLevel, message: string) {
      //   console.log(message);
      // },
      logLevel: LogLevel.Verbose,
      piiLoggingEnabled: false,
    },
  },
};

/**
 * An optional silentRequest object can be used to achieve silent SSO
 * between applications by providing a "login_hint" property.
 */
export const silentRequest = {
  scopes: ['openid', 'profile'],
  loginHint: 'example@domain.net',
};

/**
 * Add here the endpoints and scopes when obtaining an access token for protected web APIs. For more information, see:
 * https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-browser/docs/resources-and-scopes.md
 */
export const protectedResources = {
  bohoTenantsApi: {
    endpoint: environment.apiUrl + 'Tenant',
    scopes: [
      `https://${environment.azure_ad_b2c}.onmicrosoft.com/${environment.api_clientId}/access_as_user`,
    ],
  },
  bohoUsersApi: {
    endpoint: environment.apiUrl + 'User',
    scopes: [
      `https://${environment.azure_ad_b2c}.onmicrosoft.com/${environment.api_clientId}/access_as_user`,
    ],
  },
  bohoRolesApi: {
    endpoint: environment.apiUrl + 'Role',
    scopes: [
      `https://${environment.azure_ad_b2c}.onmicrosoft.com/${environment.api_clientId}/access_as_user`,
    ],
  },
  bohoWidgetsApi: {
    endpoint: environment.apiUrl + 'Widget',
    scopes: [
      `https://${environment.azure_ad_b2c}.onmicrosoft.com/${environment.api_clientId}/access_as_user`,
    ],
  },
};

/**
 * Scopes you add here will be prompted for user consent during sign-in.
 * By default, MSAL.js will add OIDC scopes (openid, profile, email) to any login request.
 * For more information about OIDC scopes, visit:
 * https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-permissions-and-consent#openid-connect-scopes
 */
export const loginRequest = {
  scopes: [],
};
