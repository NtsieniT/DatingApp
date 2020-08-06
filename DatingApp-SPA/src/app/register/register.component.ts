import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';

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

  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  register()
  {
    this.authService.register(this.model).subscribe( () => {
      console.log('registration successful');
    },
    error => {
      console.log(error);
    })
  }

  cancel(){
    // emit from method using .emit
    this.cancelRegister.emit(false);
    console.log('cancelled');
  }
}
