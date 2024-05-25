import {Component, Input, SimpleChanges} from '@angular/core';
import {animate, state, style, transition, trigger} from "@angular/animations";
import {AuthService} from "../../../auth.service";
import {DatePipe, NgClass, NgForOf, NgIf, NgOptimizedImage} from "@angular/common";
import {FormsModule} from "@angular/forms";
import {interval, startWith, Subscription, switchMap} from "rxjs";
import {Organization} from "../../../DataTypes/organization";
import {User} from "../../../DataTypes/user";
import {Event} from "../../../DataTypes/event";
import {GroupedEvent} from "../../../DataTypes/grouped.event";
import {DataService} from "../../../DataTypes/data.service";

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    DatePipe,
    FormsModule,
    NgOptimizedImage,
    NgIf,
    NgForOf,
    NgClass,
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css',
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
export class DashboardComponent {
  // Current Organization
  @Input()
  org: Organization | undefined;

  // Input-Fields
  titel: string = "";
  description?: string;
  start: Date = new Date();
  end: Date = new Date();

  // Displayed List of Events and Users
  events: Event[] = new Array<Event>;
  users: Array<User> = new Array<User>();
  // Interval
  subscription: Subscription = new Subscription();

  // Animation-Info, if AddEvent-Part is expanded
  isExpandedAddEvent: boolean = false;

  constructor(private authService: AuthService) {}

  ngOnInit(){
    // Starts Interval
    this.startPolling();
  }

  ngOnDestroy(){
    // Ends Interval
    this.stopPolling();
  }

  // Gets every 5 Seconds List of Events and Users of Organization
  startPolling() {
    // Gets Events of Organization
    this.subscription = interval(5000).pipe(
      startWith(0),
      switchMap(() => this.getEventsOrganization())
    ).subscribe(
      (response: any) => {
        this.events = response;
      }
    );

    // Gets Users of Organization
    this.subscription = interval(5000).pipe(
      startWith(0),
      switchMap(() => this.getUserList())
    ).subscribe(
      (response: any) => {
        this.users = response;
      }
    );
  }

  // Operation Of getting Events
  getEventsOrganization(){
    let orgId = this.org?.id;

    if(orgId != null){
      return this.authService.geteventsorganization(orgId);
    }

    return [];
  }

  // Operation Of getting Users
  getUserList(){
    let orgId = this.org?.id;

    if(orgId != null){
      return  this.authService.getuserlist(orgId);
    }

    return [];
  }

  // Stops Interval
  stopPolling() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  // When different Organization Gets Selected, that Values of new selected Organization are displayed
  ngOnChanges(changes: SimpleChanges){
    if(changes['org']) {
      this.events = new Array<Event>();
      this.users = new Array<User>();

      let orgId = this.org?.id;

      if(orgId != null){
        // Gets Events
        this.authService.geteventsorganization(orgId).subscribe(events => this.events = events);
        // Gets Users
        this.authService.getuserlist(orgId).subscribe(users => this.users = users);
      }
    }
  }

  // Groups
  get groupedEvents(){
    const grouped: { [date: string]: GroupedEvent } = {};

    // Sorts Events after Dates
    this.events.forEach(event => {
      // Gets Date
      const dateKey = event.datetimestart.toString().split('T')[0];
      // If Date doesn't exist it gets added
      if (!grouped[dateKey]) {
        grouped[dateKey] = { date: event.datetimestart, events: [] };
      }
      // Event gets Pushed to Their Date
      grouped[dateKey].events.push(event);
      // Sorting Start time upwards
      grouped[dateKey].events.sort((a: Event, b: Event) => {
        // Checks if smaller or bigger than Time Before
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
    // Checks If every needed Input Is answered
    if(this.titel != "" && this.org?.id != undefined) {
      // Checks if Start-Datetime is smaller than End-Datetime
      if(this.start<=this.end)
      {
        if(this.description == ""){
          this.description = undefined;
        }
        // Creates Event-Object
        let newEvent: Event = {
          titel: this.titel,
          description: this.description,
          datetimestart: this.start,
          datetimeend: this.end,
          organizationid: this.org?.id,
        }

        // Adds Event to Organization
        this.authService.addEvent(newEvent).subscribe(
          (response: any) => {
            alert("Added Event successfully!");
          },
          (error) => {
            alert("Something went wrong!");
          });
      }
      else {
        alert("End-Date has to be bigger or the same");
      }

    }
    else{
      alert("Event got to have a titel!");
    }

    // Removes Input For GUI
    this.titel = "";
    this.description = "";
    this.start = new Date();
    this.end = new Date();
  }

  promoteUser(userid: string|undefined){
    // Request so selected User gets Promoted to Admin-Status
    this.authService.promoteUser(userid, this.org?.id, DataService.user?.id).subscribe(worked => {
      // Validates If worked
      if(worked){
        alert("Promoted User");
      }
      else {
        alert("You do not have the rights!")
      }
    });
  }

  removeUserFromOrganization(userid: string|undefined) {
    // Request, that User gets Removed from Organization
    this.authService.removeuserorganization(userid, this.org?.id, DataService.user?.id).subscribe(worked => {
      // Validates If Removed User
      if(worked){
        alert("Removed User")
      }
      else {
        alert("You do not have the rights!")
      }
    });
  }
}
