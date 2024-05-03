import {Component, Input, SimpleChanges} from '@angular/core';
import {AuthService, Organization, Event, User, DataService} from "../../auth.service";
import {DatePipe, NgForOf, NgIf, NgOptimizedImage} from "@angular/common";

@Component({
  selector: 'app-organization',
  standalone: true,
  imports: [
    NgForOf,
    NgIf,
    NgOptimizedImage,
    DatePipe
  ],
  templateUrl: './organization.component.html',
  styleUrl: './organization.component.css'
})
export class OrganizationComponent {
  @Input()
  org: Organization | undefined;

  events: Event[] = new Array<Event>;
  users: Array<User> = new Array<User>();

  constructor(private authService: AuthService) {}

  ngOnChanges(changes: SimpleChanges){
    if(changes['org']) {
      this.events = new Array<Event>();
      let orgId = this.org?.id;
      if(orgId != null){
        this.authService.geteventsorganization(orgId).subscribe(events => {
          this.events = events
          //this.groupedEvents();
        });
        this.authService.getuserlist(orgId).subscribe(users => this.users = users);
      }
    }
  }

  get groupedEvents(){
    const grouped: { [date: string]: GroupedEvent } = {};
    this.events.forEach(event => {
      const dateKey = event.datetimestart.toString().split('T')[0]; // Grouping by date only
      if (!grouped[dateKey]) {
        grouped[dateKey] = { date: event.datetimestart, events: [] };
      }
      grouped[dateKey].events.push(event);
    });
    return Object.values(grouped);
  }

  promoteUser(userid: string|undefined){
    this.authService.promoteUser(userid, this.org?.id, DataService.data).subscribe(worked => {
      if(worked){
        console.log("Worked");
      }
      else {
        console.log("Did not work!")
      }
    });
  }

  removeUserFromOrganization(userid: string|undefined) {
    this.authService.removeuserorganization(userid, this.org?.id, DataService.data).subscribe(worked => {
      if(worked){
        console.log("Worked");
      }
      else {
        console.log("Did not work!")
      }
    });
  }

}

interface GroupedEvent {
  date: Date;
  events: any[]; // Adjust this type according to your event structure
}

