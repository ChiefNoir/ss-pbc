// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  apiEndpoint: 'http://localhost:5000/api/v1/',
  authEndpoint: 'http://localhost:5000/api/v1/',
  siteName: 'Fireplace Of Despair (dev)',
  paging: {
    maxUsers: 3,
    maxProjects: 3
  }
};
