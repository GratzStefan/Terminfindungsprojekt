import { Component } from '@angular/core';
import {FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators} from "@angular/forms";
import {AuthService} from "../auth.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [
    FormsModule,
    ReactiveFormsModule
  ],
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.css'
})
export class SignupComponent {
  form: FormGroup = this.fb.group({

    firstname: ['', Validators.required],
    lastname: ['', Validators.required],
    username: ['', Validators.required],
    password: ['', Validators.required],
  });
  constructor(
    private authService: AuthService,
    private fb: FormBuilder,
    private router: Router
  ) {}

  signup(){
    let user = this.authService.signup(
      this.form.value.firstname,
      this.form.value.lastname,
      this.form.value.username,
      this.form.value.password,
    );

    user.subscribe(data => {
        alert('User created!')
      },
      err => {
        alert('User already exists or something went wrong!');
      });
  }

  login(){
    this.router.navigateByUrl("/login");
  }
}
