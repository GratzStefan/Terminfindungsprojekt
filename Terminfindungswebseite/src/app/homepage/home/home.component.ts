import {Component, SimpleChanges} from '@angular/core';
import {DatePipe, NgForOf, NgIf} from "@angular/common";
import {AuthService} from "../../auth.service";
import {interval, startWith, Subscription, switchMap} from "rxjs";
import {GroupedEvent} from "../../DataTypes/grouped.event";
import {Event} from "../../DataTypes/event";

@Component({
  selector: 'app-home',
  standalone: true,
    imports: [
        DatePipe,
        NgForOf,
        NgIf
    ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  // Displayed List Of Events
  events: Event[] = new Array<Event>;
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

  // Gets Events of User every 5 Seconds
  startPolling() {
    this.subscription = interval(5000).pipe(
      startWith(0),
      switchMap(() => this.authService.getEventsOfUser())
    ).subscribe(
      (response: any) => {
        this.events = response;
      }
    );
  }

  // Ends Interval
  stopPolling() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

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
}
