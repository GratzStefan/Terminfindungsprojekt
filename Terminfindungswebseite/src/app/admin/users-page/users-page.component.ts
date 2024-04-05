import { Component } from '@angular/core';
import {RouterLink} from "@angular/router";
import {AuthService} from "../../auth.service";

@Component({
  selector: 'app-users-page',
  standalone: true,
  imports: [
    RouterLink
  ],
  templateUrl: './users-page.component.html',
  styleUrl: './users-page.component.css'
})
export class UsersPageComponent {

  constructor(private authService: AuthService) {
  }
  logout() {
    this.authService.logout();
  }
}
