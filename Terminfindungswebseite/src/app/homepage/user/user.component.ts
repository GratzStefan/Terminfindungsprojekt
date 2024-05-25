import { Component } from '@angular/core';
import {DatePipe, NgForOf, NgIf} from "@angular/common";
import {AuthService} from "../../auth.service";
import {FormsModule} from "@angular/forms";
import {interval, startWith, Subscription, switchMap} from "rxjs";
import {DataService} from "../../DataTypes/data.service";
import {User} from "../../DataTypes/user";
import {Request} from "../../DataTypes/request";
import {StatusType} from '../../DataTypes/status.type';

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
  // Input-Values
  username: string = DataService.user?.username;
  firstname: string = DataService.user?.firstname;
  lastname: string = DataService.user?.lastname;

  // Display List of Requests
  requests: Request[] = new Array<Request>;
  // Interval
  subscription: Subscription = new Subscription();

  constructor(private authService: AuthService) {}

  ngOnInit(){
    // Starts Interval
    this.startPolling();
  }

  ngOnDestroy(){
    // Ends Interval
    this.stopPolling();
  }

  // Interval gets Requests Of User every 5 Seconds
  startPolling() {
    // Initialize Interval
    this.subscription = interval(5000).pipe(
      startWith(0),
      switchMap(() => this.authService.getAllRequestsOfUser())
    ).subscribe(
      (response: any) => {
        // Assign so it gets Displayed in GUI
        this.requests = response;
      }
    );
  }

  // Interval gets Stopped
  stopPolling() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  // Click-Event, which changes User-Information
  changeUserInformation() {
    // Create User-Object
    let user: User = {
      id: DataService.user?.id,
      username: this.username,
      firstname: this.firstname,
      lastname: this.lastname,
    }

    // Requests REST-API to Change User-Information
    this.authService.changeUser(user).subscribe(data => {
      // Checks if changed
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
