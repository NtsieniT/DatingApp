import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { JwtHelperService} from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import { BehaviorSubject } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class AuthService {


  baseUrl = environment.apiUrl + 'auth/';

  /*Helper library for handling JWTs in Angular 2+ apps */
  jwtHelper = new JwtHelperService();
  decodedToken: any;

  currentUser: User;

  photoUrl = new BehaviorSubject<string>('../../assets/user.png');
  currentPhotoUrl = this.photoUrl.asObservable();

constructor(private http: HttpClient) { }

changeMemberPhoto(photoUrl: string): any{
  this.photoUrl.next(photoUrl);
}

login(model: any): any
{
  // pipe allows us to chain rsjx operators to our request.
  return this.http.post(this.baseUrl + 'login', model)
    .pipe(
      map((response: any) => {
        const user = response;
        if (user)
        {
          // set and store token on the local storage
          localStorage.setItem('token', user.token);

          // use JSON.stringify to convert object to string
          localStorage.setItem('user', JSON.stringify(user.user));

          this.decodedToken = this.jwtHelper.decodeToken(user.token);
          this.currentUser = user.user;
          this.changeMemberPhoto(this.currentUser.photoUrl);
        }
      })
    );
}

register(user: User): any
{
  return this.http.post(this.baseUrl + 'register', user);

}

loggedIn(): any {
  const token = localStorage.getItem('token');
  return !this.jwtHelper.isTokenExpired(token); // return if token is not expired
}

}
