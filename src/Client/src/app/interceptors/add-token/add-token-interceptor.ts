import {HttpInterceptorFn} from '@angular/common/http';
import {inject} from '@angular/core';
import {environment} from '../../environment/environment';
import {Auth} from '../../services/auth/auth';
import {CreateDreamPopUp} from '../../components/dream/create-dream-pop-up/create-dream-pop-up';

CreateDreamPopUp

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
