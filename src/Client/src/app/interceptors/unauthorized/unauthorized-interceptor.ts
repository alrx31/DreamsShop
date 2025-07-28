import { HttpInterceptorFn } from '@angular/common/http';
import {catchError, throwError} from 'rxjs';
import {Router} from '@angular/router';
import {inject} from '@angular/core';

export const unauthorizedInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router)
  return next(req).pipe(
    catchError((err) => {
      if(err.status === 401){
        router.navigate(['/login']);
      }

      return throwError(()=>err);
    })
  );
};
