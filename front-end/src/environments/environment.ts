// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment =
{
  production: false,
  apiEndpoint: 'https://localhost:44386/api/v1/',
  authEndpoint: 'https://localhost:44386/api/v1/',
  siteName: 'Fireplace Of Despair (dev)',
  footerCopyright: 'Chief',
  paging:
  {
    maxUsers: 3,
    maxProjects: 3
  }
};
