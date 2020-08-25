import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  // @Input recieves properties into child component from parent component.
  // @Input() valuesFromHome: any;

  // @Output emit events
  @Output() cancelRegister = new EventEmitter();

  model: any = {}

  constructor(private authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
  }

  register()
  {
    this.authService.register(this.model).subscribe( () => {
      this.alertify.success('Registered successfully');
    },
    error => {
      this.alertify.error(error);
    });
  }

  cancel(){
    // emit from method using .emit
    this.cancelRegister.emit(false);
    this.alertify.message('Calcelled');
  }
}
