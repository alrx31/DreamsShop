import { HttpInterceptorFn } from '@angular/common/http';
import {inject} from '@angular/core';
import {Store} from '@ngrx/store';
import {environment} from '../environment/environment';
import {Router} from '@angular/router';
import {Auth} from '../services/auth';

export const addTokenInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(Auth);
  const token = localStorage.getItem(environment.accessTokenName);

  if (token) {
    req = req.clone({
      headers: req.headers.set('Authorization', `Bearer ${token}`)
    });
  }else{
    authService.logout();
  }

  return next(req);
};
