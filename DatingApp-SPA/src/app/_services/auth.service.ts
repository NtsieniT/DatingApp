import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {


  baseUrl = 'http://localhost:5000/api/auth/';

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
          localStorage.setItem('token', user.token)
        }
      })
    )
}

register(model: any)
{
  return this.http.post(this.baseUrl + 'register', model);

}

}