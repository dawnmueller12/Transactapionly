// This file can be replaced during build by using the `fileReplacements` array.
// `ng build ---prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  apiUrl: 'http://52.226.201.84/api/',
  web_clientId: '28db9c11-48f4-4a00-ab13-66ba215c977e',
  api_clientId: 'ada019b2-d3a5-4870-ae04-43943a0ecbd3',
  signin_flow_name: 'transact-signin',
  signup_flow_name: 'transact-signup',
  azure_ad_b2c: 'TransactDev',
};

/*
 * In development mode, to ignore zone related error stack frames such as
 * `zone.run`, `zoneDelegate.invokeTask` for easier debugging, you can
 * import the following file, but please comment it out in production mode
 * because it will have performance impact when throw error
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
