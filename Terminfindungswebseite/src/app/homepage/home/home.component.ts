import {Component, SimpleChanges} from '@angular/core';
import {DatePipe, NgForOf, NgIf} from "@angular/common";
import {AuthService, Event, GroupedEvent, User} from "../../auth.service";

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
  events: Event[] = new Array<Event>;

  constructor(private authService: AuthService) {}

  ngOnInit(){
    this.events = new Array<Event>();
    this.authService.getEventsOfUser().subscribe(events => this.events = events);
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
}
