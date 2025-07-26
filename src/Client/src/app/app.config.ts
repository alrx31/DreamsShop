import {ApplicationConfig, provideBrowserGlobalErrorListeners, provideZoneChangeDetection} from '@angular/core';
import {provideRouter} from '@angular/router';

import {routes} from './app.routes';
import {provideHttpClient, withInterceptors} from '@angular/common/http';
import {addTokenInterceptor} from './interceptors/add-token/add-token-interceptor';
import {provideAnimations} from '@angular/platform-browser/animations';
import {unauthorizedInterceptor} from './interceptors/unauthorized/unauthorized-interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideAnimations(),
    provideHttpClient(withInterceptors([
      addTokenInterceptor,
      unauthorizedInterceptor
    ]))
  ]
};
