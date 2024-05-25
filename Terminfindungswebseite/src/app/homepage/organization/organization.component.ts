import {Component, Input} from '@angular/core';
import {AuthService} from "../../auth.service";
import {DatePipe, NgClass, NgForOf, NgIf, NgOptimizedImage, NgSwitch, NgSwitchCase} from "@angular/common";

import {FormsModule} from "@angular/forms";
import {CreateComponent} from "../create/create.component";
import {HomeComponent} from "../home/home.component";
import {SearchComponent} from "../search/search.component";
import {UserComponent} from "../user/user.component";
import {DashboardComponent} from "./dashboard/dashboard.component";
import {NotificationComponent} from "./notification/notification.component";
import {Organization} from "../../DataTypes/organization";
import {OrganizationComponentType} from "../../DataTypes/organization.component.type";

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
  // Organization Component Window
  type: OrganizationComponentType = OrganizationComponentType.Dashboard;

  // Current Organization
  @Input()
  org: Organization | undefined;

  constructor(private authService: AuthService) {}

  // Deletes Organization
  deleteOrganization(){
    if(this.org != undefined){
      // Sends Request to Delete current Organization
      this.authService.deleteOrganization(this.org).subscribe(deletedCount => {
        // Output for User, if deleted
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
