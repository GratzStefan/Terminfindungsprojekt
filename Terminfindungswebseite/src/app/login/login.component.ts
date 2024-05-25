import { Component } from '@angular/core';
import { AuthService} from "../auth.service";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {JsonPipe} from "@angular/common";
import {Router} from "@angular/router";

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    JsonPipe
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  // Input-Fields (User-Information)
  form: FormGroup = this.fb.group({
    username: ['', Validators.required],
    password: ['', Validators.required],
  });

  constructor(
    private authService: AuthService, private fb: FormBuilder, private router: Router) {}

  // Login
  login(){
    this.authService.login(this.form.value.username, this.form.value.password);
  }

  // Navigates to SignUp-Page
  signup() {
    this.router.navigateByUrl('/signup');
  }
}
