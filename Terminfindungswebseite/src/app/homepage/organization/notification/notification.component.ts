import {Component, Input, SimpleChanges} from '@angular/core';
import {FormsModule} from "@angular/forms";
import {NgForOf, NgClass, NgIf} from "@angular/common";
import {AuthService} from "../../../auth.service";
import {interval, startWith, Subscription, switchMap} from "rxjs";
import {Organization} from "../../../DataTypes/organization";
import {Request} from "../../../DataTypes/request";
import { StatusType } from '../../../DataTypes/status.type';

@Component({
  selector: 'app-notification',
  standalone: true,
  imports: [
    FormsModule,
    NgForOf,
    NgIf,
    NgClass,
  ],
  templateUrl: './notification.component.html',
  styleUrl: './notification.component.css'
})
export class NotificationComponent {
  // Current Organization
  @Input()
  org: Organization | undefined;
  // Displayed List of Requests
  requests: Request[] = new Array<Request>();
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

  // Gets Request To Organization every 5 Seconds
  startPolling() {
    // GET-Request For Requests To Organization
    this.subscription = interval(5000).pipe(
      startWith(0),
      switchMap(() => this.getRequests())
    ).subscribe(
      (response: any) => {
        this.requests = response;
      }
    );
  }

  // Stops Interval
  stopPolling() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  // When different Organization Gets Selected, that Values of new selected Organization are displayed
  ngOnChanges(changes: SimpleChanges) {
    if(changes['org']) {
      this.requests = new Array<Request>();
      let orgId = this.org?.id;
      if(orgId != null){
        // Gets requests
        this.authService.getAllRequestsToOrganization(orgId).subscribe(requests => {
          this.requests = requests
        });
      }
    }
  }

  // Operation Of Getting Request-Data
  getRequests(){
    let orgId = this.org?.id;

    if(orgId != null) {
      return this.authService.getAllRequestsToOrganization(orgId);
    }

    return [];
  }

  // Click-Event, which decides, if Person is Accepted or Denied to Access Organization
  changeStatus(request: Request, type: StatusType) {
    // Change Request-Status
    request.status = type;

    // Request to Change Request-Status
    this.authService.changeStatus(request).subscribe(data => {
      // Output for user, if worked
      if(data==1) {
        alert("Changed status successfully!");
        this.requests = this.requests.filter(r => r.id !== request.id);
      }
      else {
        alert("Something failed!");
      }
    });
  }

  protected readonly StatusType = StatusType;
}
