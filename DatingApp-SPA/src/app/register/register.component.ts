import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { User } from '../_models/user';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  // @Input recieves properties into child component from parent component.
   @Input() valuesFromHome: any;

  // @Output emit events
  @Output() cancelRegister = new EventEmitter();

  user: User;

  registerForm: FormGroup;

  // provides Configuration for bootstraps date picker
  // partial class makes all properties in the type optional
  bsConfig: Partial<BsDatepickerConfig>;

  constructor(private authService: AuthService,
              private alertify: AlertifyService,
              private formBuilder: FormBuilder,
              private router: Router) { }

  ngOnInit(): any {
    this.bsConfig = {
      containerClass: 'theme-default'
    };
    this.createRegisterForm();


  }

  // this method builds a form using form builder of all fields to display on the form
  createRegisterForm(): any{
    this.registerForm = this.formBuilder.group({
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: [null, Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['', [ Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', Validators.required]

    }, {validator: this.passwordMatchValidator});
  }

  // method will be used to compare password if they match or not in the form group
  passwordMatchValidator(g: FormGroup): any{
    return  g.get('password').value ===
            g.get('confirmPassword').value ? null : {mismatch: true};
  }

  register(): any
  {
    if (this.registerForm.valid){
      // Object.assign typically clones the object from this.registerForm.value and assigns to the 
      // empty object which then assigns to the user model
      this.user = Object.assign({}, this.registerForm.value);

      this.authService.register(this.user).subscribe( () => {
        this.alertify.success('Registered successfully');
      },
      error => {
        this.alertify.error(error);
      },
        () => this.authService.login(this.user).subscribe(() => {
          this.router.navigate(['members']);
        })
      );

    }
  }

  cancel(): any{
    // emit from method using .emit
    this.cancelRegister.emit(false);
    this.alertify.message('Calcelled');
  }
}
