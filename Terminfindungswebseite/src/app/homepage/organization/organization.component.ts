import {Component, Input} from '@angular/core';
import {AuthService, Organization} from "../../auth.service";
import {DatePipe, NgClass, NgForOf, NgIf, NgOptimizedImage, NgSwitch, NgSwitchCase} from "@angular/common";

import {FormsModule} from "@angular/forms";
import {CreateComponent} from "../create/create.component";
import {HomeComponent} from "../home/home.component";
import {SearchComponent} from "../search/search.component";
import {UserComponent} from "../user/user.component";
import {DashboardComponent} from "./dashboard/dashboard.component";
import {NotificationComponent} from "./notification/notification.component";

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
    CreateComponent,
    HomeComponent,
    NgSwitchCase,
    SearchComponent,
    UserComponent,
    NgSwitch,
    DashboardComponent,
    NotificationComponent,
  ],
  templateUrl: './organization.component.html',
  styleUrl: './organization.component.css',
})


export class OrganizationComponent {
  type: OrganizationComponentType = OrganizationComponentType.Dashboard;

  @Input()
  org: Organization | undefined;

  constructor(private authService: AuthService) {}

  deleteOrganization(){
    if(this.org != undefined){
      this.authService.deleteOrganization(this.org).subscribe(deletedCount => {
        if(deletedCount == 1){
          alert("Successfully deleted organization!");
        }
        else {
          alert("Something went wrong!")
        }
      });
    }
  }

  protected readonly OrganizationComponentType = OrganizationComponentType;
}

enum OrganizationComponentType {
  Dashboard,
  Notification
}
