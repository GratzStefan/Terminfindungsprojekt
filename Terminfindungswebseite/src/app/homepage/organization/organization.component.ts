import {Component, Input, SimpleChanges} from '@angular/core';
import {AuthService, Organization, Event, User, DataService} from "../../auth.service";
import {DatePipe, NgClass, NgForOf, NgIf, NgOptimizedImage} from "@angular/common";

import {animate, state, style, transition, trigger} from "@angular/animations";
import {FormsModule} from "@angular/forms";

@Component({
  selector: 'app-organization',
  standalone: true,
  providers: [],
  imports: [
    NgForOf,
    NgIf,
    NgOptimizedImage,
    DatePipe,
    NgClass,
    FormsModule,
  ],
  templateUrl: './organization.component.html',
  styleUrl: './organization.component.css',
  animations: [
    trigger('expandCollapse', [
      state('hidden', style({
        height: '55px',
        opacity: 1
      })),
      state('expanded', style({
        height: '*',
        opacity: 1
      })),
      transition('hidden <=> expanded', [
        animate('0.5s')
      ]),
    ]),
  ]
})


export class OrganizationComponent {
  @Input()
  org: Organization | undefined;

  titel: string = "";
  description?: string;
  start: Date = new Date();
  end: Date = new Date();

  events: Event[] = new Array<Event>;
  users: Array<User> = new Array<User>();

  isExpandedAddEvent: boolean = false;


  constructor(private authService: AuthService) {}

  ngOnChanges(changes: SimpleChanges){
    if(changes['org']) {
      this.events = new Array<Event>();
      let orgId = this.org?.id;
      if(orgId != null){
        this.authService.geteventsorganization(orgId).subscribe(events => this.events = events);
        this.authService.getuserlist(orgId).subscribe(users => this.users = users);
        /* -- Test Data --
        this.users = [
          { username: "test", lastname: "hi", firstname: "test", password: "test"},
          { username: "test", lastname: "hi", firstname: "test", password: "test"},
          { username: "test", lastname: "hi", firstname: "test", password: "test"},
          { username: "test", lastname: "hi", firstname: "test", password: "test"},
          { username: "test", lastname: "hi", firstname: "test", password: "test"},
          { username: "test", lastname: "hi", firstname: "test", password: "test"},
          { username: "test", lastname: "hi", firstname: "test", password: "test"},
          { username: "test", lastname: "hi", firstname: "test", password: "test"},
          { username: "test", lastname: "hi", firstname: "test", password: "test"},
          { username: "test", lastname: "hi", firstname: "test", password: "test"},
          { username: "test", lastname: "hi", firstname: "test", password: "test"},
          { username: "test", lastname: "hi", firstname: "test", password: "test"},
          { username: "test", lastname: "hi", firstname: "test", password: "test"},
        ];*/
      }
    }
  }

  get groupedEvents(){
    const grouped: { [date: string]: GroupedEvent } = {};
    this.events.forEach(event => {
      const dateKey = event.datetimestart.toString().split('T')[0];
      if (!grouped[dateKey]) {
        grouped[dateKey] = { date: event.datetimestart, events: [] };
      }
      grouped[dateKey].events.push(event);
    });
    return Object.values(grouped);
  }

  addEvent() {
    if(this.titel != "" && this.org?.id != undefined) {
      if(this.start<this.end)
      {
        if(this.description == ""){
          this.description = undefined;
        }
        let newEvent: Event = {
          titel: this.titel,
          description: this.description,
          datetimestart: this.start,
          datetimeend: this.end,
          organizationid: this.org?.id,
        }

        this.authService.addEvent(newEvent).subscribe(value => console.log(value));
        this.authService.geteventsorganization(this.org?.id).subscribe(events => this.events = events);
        alert("Added Event successfully!");
      }
      else {
        alert("End-Date has to be smaller or the same");
      }
    }
    else{
      alert("Event got to have a titel!");
    }
  }

  promoteUser(userid: string|undefined){
    this.authService.promoteUser(userid, this.org?.id, DataService.data).subscribe(worked => {
      if(worked){
        alert("Promoted User");
      }
      else {
        alert("You do not have the rights!")
      }
    });
  }

  removeUserFromOrganization(userid: string|undefined) {
    this.authService.removeuserorganization(userid, this.org?.id, DataService.data).subscribe(worked => {
      if(worked){
        alert("Removed User")
        this.authService.getuserlist(this.org?.id).subscribe(users => this.users = users);
      }
      else {
        alert("You do not have the rights!")
      }
    });
  }
}

interface GroupedEvent {
  date: Date;
  events: any[];
}
