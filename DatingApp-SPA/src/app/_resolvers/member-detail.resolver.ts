import { catchError } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import { AlertifyService } from './../_services/alertify.service';
import { UserService } from './../_services/user.service';
import {Injectable} from '@angular/core';
import {User} from '../_models/user';
import {Resolve, Router, ActivatedRouteSnapshot} from '@angular/router';

/**
 * Resolvers helps load data before data can be loaded in the component
 * Resolves automatically subscribes to the method.
 */

@Injectable()
export class MemberDetailResolver implements Resolve<User>{

    constructor(private userService: UserService, private router: Router,
                private alertify: AlertifyService){}

    resolve(route: ActivatedRouteSnapshot): Observable<User>{
        return this.userService.getUser(route.params.id).pipe(
            catchError(error => {
                this.alertify.error('Problem retrieving data');
                this.router.navigate(['/members']);
                return of(null); // of returns observable of null
            } )
        );
    }

}
