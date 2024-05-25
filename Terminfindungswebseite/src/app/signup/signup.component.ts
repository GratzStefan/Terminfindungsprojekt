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
  // Input-Fields
  form: FormGroup = this.fb.group({
    firstname: ['', Validators.required],
    lastname: ['', Validators.required],
    username: ['', Validators.required],
    password: ['', Validators.required],
  });

  constructor(private authService: AuthService, private fb: FormBuilder, private router: Router) {}

  // Creating new User
  signup(){
    // Signing Up a new User
   this.authService.signup(
      this.form.value.firstname,
      this.form.value.lastname,
      this.form.value.username,
      this.form.value.password,
    ).subscribe((response: any) => {
        // Displaying  when Creating Worked
        alert('User created!')
      },
      err => {
        // Displaying If something went wrong
        alert('User already exists or something went wrong!');
      });
  }

  // Navigates to Login-Page
  login(){
    this.router.navigateByUrl("/login");
  }
}
