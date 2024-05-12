import { Component } from '@angular/core';
import {DatePipe, NgForOf, NgIf} from "@angular/common";
import {AuthService, DataService, Request, StatusType, User} from "../../auth.service";
import {FormsModule} from "@angular/forms";
import {last} from "rxjs";

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [
    DatePipe,
    NgForOf,
    NgIf,
    FormsModule
  ],
  templateUrl: './user.component.html',
  styleUrl: './user.component.css'
})
export class UserComponent {
  username: string = DataService.user?.username;
  firstname: string = DataService.user?.firstname;
  lastname: string = DataService.user?.lastname;


  requests: Request[] = new Array<Request>;

  constructor(private authService: AuthService) {}

  ngOnInit(){
    this.requests = new Array<Request>();
    this.authService.getAllRequestsOfUser().subscribe(requests => this.requests = requests);
  }

  changeUserInformation() {
    let user: User = {
      id: DataService.user?.id,
      username: this.username,
      firstname: this.firstname,
      lastname: this.lastname,
    }

    this.authService.changeUser(user).subscribe(data => {
      if(data==1) {
        DataService.user = user;
        alert("Successfully changed user");
      }
      else {
        alert("Something went wrong!");
      }
    });
  }

  protected readonly StatusType = StatusType;
}
