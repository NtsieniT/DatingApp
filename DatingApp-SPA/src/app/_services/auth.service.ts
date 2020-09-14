import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { JwtHelperService} from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {


  baseUrl = environment.apiUrl + 'auth/';

  /*Helper library for handling JWTs in Angular 2+ apps */
  jwtHelper = new JwtHelperService();

  decodedToken: any;

constructor(private http: HttpClient) { }

login(model: any)
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
          this.decodedToken = this.jwtHelper.decodeToken(user.token);
        }
      })
    );
}

register(model: any)
{
  return this.http.post(this.baseUrl + 'register', model);

}

loggedIn() {
  const token = localStorage.getItem('token');
  return !this.jwtHelper.isTokenExpired(token); // return if token is not expired
}

}
