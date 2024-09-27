import { HttpErrorResponse, HttpInterceptorFn, HttpEvent } from '@angular/common/http';
import { inject } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { SnackbarService } from '../services/snackbar.service';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const snack = inject(SnackbarService);
  return next(req).pipe(
    catchError((err: HttpErrorResponse) => {
      if (err.status === 400) {
        if (err.error.errors) {
          const modelStateErrors = [];
          for (const key in err.error.errors) {
              if(err.error.errors[key]) {
                modelStateErrors.push(err.error.errors[key])
              }
          }
          throw modelStateErrors.flat();
        }

      }
      if (err.status === 401) {
        snack.error(err.error.title || err.error)
      }
      if (err.status === 404) {
        // router.navigateByUrl('/not-found');
        router.navigateByUrl('/not-found'); 
      }
      if (err.status === 500) {
        // router.navigateByUrl('/server-error');
        const navigationExtras: NavigationExtras = {state: {err: err.error}};
        router.navigateByUrl('server-error',navigationExtras);
        
      }
      return throwError(() => err);
    })
  );

};
