import {Component} from '@angular/core';
import {ɵEmptyOutletComponent, RouterLink, RouterOutlet } from "@angular/router";
import {AuthService} from "../auth.service";
import {SearchComponent} from "./search/search.component";
import {OrganizationComponent} from "./organization/organization.component";
import {NgForOf, NgOptimizedImage, NgSwitch, NgSwitchCase} from "@angular/common";
import {CreateComponent} from "./create/create.component";
import {HomeComponent} from "./home/home.component";
import {UserComponent} from "./user/user.component";
import {interval, startWith, Subscription, switchMap} from "rxjs";
import {Organization} from "../DataTypes/organization";
import { DataService } from '../DataTypes/data.service';
import { ComponentType } from '../DataTypes/component.type';

@Component({
  selector: 'app-homepage',
  standalone: true,
  imports: [
    RouterOutlet,
    RouterLink,
    SearchComponent,
    ɵEmptyOutletComponent,
    NgSwitch,
    NgSwitchCase,
    CreateComponent,
    NgForOf,
    NgOptimizedImage,
    OrganizationComponent,
    HomeComponent,
    UserComponent
  ],
  templateUrl: './homepage.component.html',
  styleUrl: './homepage.component.css',
})
export class HomepageComponent {
  // Type, which Component opened
  type: ComponentType = ComponentType.Default;
  // Displayed List Of Organizations
  orgs: Organization[] = new Array<Organization>();
  // Which Organization selected, if selected
  curOrg: Organization | undefined;
  // Subscription for Interval
  subscription: Subscription = new Subscription();

  constructor(protected authService: AuthService) {}

  ngOnInit(){
    // Starts Interval
    this.startPolling();
  }

  ngOnDestroy(){
    // Ends Interval
    this.stopPolling();
  }

  // Interval gets Organizations Of User every 5 Seconds
  startPolling() {
    // Initialize Interval
    this.subscription = interval(5000).pipe(
      startWith(0),
      switchMap(() => {
        // Gets Organizations

        let data = DataService.user;

        if(data.id != undefined && data.id != "") {
          return this.authService.getuserorganizations(data?.id);
        }

        return [];
      })
    ).subscribe(
      (response: any) => {
        // Assign so it gets Displayed in GUI
        this.orgs = response;
      }
    );
  }

  // Interval gets Stopped
  stopPolling() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  // Click-Event on Organization (Detects clicked Organization and switches to Organization Component)
  clickedOnOrganization(org: Organization) {
    this.curOrg = org;
    this.type = ComponentType.Organization;
  }

  // So Types can be read in HTML
  protected readonly ComponentType = ComponentType;
  protected readonly DataService = DataService;
}
