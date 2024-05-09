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
      //Sorting Start time upwards
      grouped[dateKey].events.sort((a: Event, b: Event) => {
        if(a.datetimestart.toString().split('T')[1] > b.datetimestart.toString().split('T')[1]) {
          return 1;
        }
        else if(a.datetimestart.toString().split('T')[1] < b.datetimestart.toString().split('T')[1]) {
          return -1;
        }

        return 0;
      });
    });

    //Sort Dates upwards
    const dates = Object.keys(grouped).sort();
    const sortedGrouped: { [date: string]: GroupedEvent } = {};
    dates.forEach(date => {
      sortedGrouped[date] = grouped[date];
    });

    return Object.values(sortedGrouped);
  }

  addEvent() {
    if(this.titel != "" && this.org?.id != undefined) {
      if(this.start<=this.end)
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

        try{
          this.authService.addEvent(newEvent).subscribe(value =>  {
            newEvent.id = value;
            console.log(value);
          });
          this.events.push(newEvent);
          alert("Added Event successfully!");
        }
        catch {
          alert("Something went wrong!");
        }
      }
      else {
        alert("End-Date has to be bigger or the same");
      }

    }
    else{
      alert("Event got to have a titel!");
    }

    this.titel = "";
    this.description = "";
    this.start = new Date();
    this.end = new Date();
  }

  promoteUser(userid: string|undefined){
    this.authService.promoteUser(userid, this.org?.id, DataService.user?.id).subscribe(worked => {
      if(worked){
        alert("Promoted User");
      }
      else {
        alert("You do not have the rights!")
      }
    });
  }

  removeUserFromOrganization(userid: string|undefined) {
    this.authService.removeuserorganization(userid, this.org?.id, DataService.user?.id).subscribe(worked => {
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
