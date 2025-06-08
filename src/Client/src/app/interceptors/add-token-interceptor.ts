import { HttpInterceptorFn } from '@angular/common/http';
import {inject} from '@angular/core';
import {Store} from '@ngrx/store';
import {environment} from '../environment/environment';

export const addTokenInterceptor: HttpInterceptorFn = (req, next) => {
  const store = inject(Store);

  const token = localStorage.getItem(environment.accessTokenName);
  if (token) {
    req = req.clone({
      headers: req.headers.set('Authorization', `Bearer ${token}`)
    });
  }

  return next(req);
};
