import { HttpInterceptorFn } from '@angular/common/http';

export const retryInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req);
};
