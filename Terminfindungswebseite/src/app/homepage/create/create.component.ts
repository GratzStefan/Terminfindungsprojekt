import { Component } from '@angular/core';
import {NgForOf, NgSwitchCase} from "@angular/common";
import {SearchComponent} from "../search/search.component";
import {FormsModule} from "@angular/forms";
import {AuthService, DataService, Organization} from "../../auth.service";
import {OrganizationComponent} from "../organization/organization.component";
import {HomepageComponent} from "../homepage.component";

@Component({
  selector: 'app-create',
  standalone: true,
  imports: [
    NgSwitchCase,
    SearchComponent,
    FormsModule,
    NgForOf
  ],
  templateUrl: './create.component.html',
  styleUrl: './create.component.css'
})
export class CreateComponent {
  orgName: string = "";

  constructor(private authService: AuthService, private parentComponent: HomepageComponent){}

  createOrganization() {
    if(this.orgName!="") {
      let org: Organization = {
        name: this.orgName,
        creatorid: DataService.user?.id
      }

      try{
        this.authService.createorganization(org).subscribe(id => {
          org.id = id;
          this.parentComponent.orgs.push(org);
          alert("Created organization successfully!");
        });
      }
      catch {
        alert("Something went wrong!");
      }
    }
    else {
      alert("Organization must have a name!")
    }
  }
}
