// Angular HTTP Interceptor allows us to intercept http requests
// and resposes from the server and catch any errors

import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpEvent, HttpHandler, HttpRequest, HttpErrorResponse, HTTP_INTERCEPTORS } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(req).pipe(
            catchError(error => {
                if (error instanceof HttpErrorResponse){

                    // Check is user is authorized
                    if (error.status === 401){
                        return throwError(error.statusText);
                    }

                    const applicationError = error.headers.get('Application-Error');
                    if (applicationError){
                        console.error(applicationError);
                        return throwError(applicationError);

                    }
                    // This is for the http response. if modelstate error then it will be of type object
                    // else will be as serverError error
                    const serverError = error.error;
                    let modalStateErrors = '';

                    if (serverError && typeof serverError === 'object'){
                        // loop inside the object key for the serverError to build up the modalStateError
                        for (const key in serverError){
                            if (serverError[key]){
                                modalStateErrors += serverError[key] + '\n';
                            }
                        }
                    }

                    return throwError(modalStateErrors || serverError || 'Server Error');


                }

            })
        );
    }
}

// adding additional custom http interceptor to angular interceptors
export const ErrorInterceptorProvider = {
    provide: HTTP_INTERCEPTORS,
    useClass: ErrorInterceptor,
    multi: true
};
