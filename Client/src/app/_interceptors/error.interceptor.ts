import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { NavigationExtras, Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private snackBar: MatSnackBar, private router: Router) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(catchError(error => {
      if(error){
        switch(error.status){
          case 400:
            if(error.error.errors){
              const modalStateErrors = [];
              for(const key in error.error.errors){
                if(error.error.errors[key]) modalStateErrors.push(error.error.errors[key]);
              }
              for(let i of modalStateErrors){
                this.snackBar.open(i);
              }
              throw modalStateErrors.flat();
            }else if(typeof(error.error) === "object"){
              this.snackBar.open(error.error[0].description, "Ok", { duration: 3000 });
            }else{
              this.snackBar.open(error.error, "Ok", { duration: 3000 });
            }
            break;
          case 401:
            this.snackBar.open(error.error, "Ok", { duration: 3000 });
            break;
          case 404:
            this.router.navigateByUrl("/not-found");
            break;
          case 500:
            const navigationExtras: NavigationExtras = {state: {error: error.error}}
            this.router.navigateByUrl("/server-error", navigationExtras)
            break;
          default:
            this.snackBar.open("Something went wrong");
            console.log(error);
            break;
        }
      }
      return throwError(error);
    }));
  }
}
