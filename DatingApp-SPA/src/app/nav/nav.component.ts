import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = {}; // used to pass data/fields from our template to the model as objects.

  constructor(public authService: AuthService, private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {
  }

  login(): any {
    this.authService.login(this.model).subscribe(next => {
     this.alertify.success('Logged in successfully');
    },
    error => {
      this.alertify.error(error);
    },
    () => {
      this.router.navigate(['/members']);
    }
    );
  }

  loggedIn()
  {
    return this.authService.loggedIn();
  }

  logout()
  {
    localStorage.removeItem('token');
    this.alertify.message('Logged out');
    this.router.navigate(['/home']);
  }

}
