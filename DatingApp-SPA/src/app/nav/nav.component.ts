import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = {}; // used to pass data/fields from our template to the model as objects.

  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  login(): any {
    this.authService.login(this.model).subscribe(next => {
      console.log('logged in successfully');
    },
    error => {
       console.log(error);
    });
  }

  loggedIn()
  {
    const token = localStorage.getItem('token');
    return !!token; // shorthand for if statement. If true, return true else return false
  }

  logout()
  {
    localStorage.removeItem('token');
    console.log('logged out');
  }

}
