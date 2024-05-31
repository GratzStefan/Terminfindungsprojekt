import {Component, input} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {AuthService} from "../../auth.service";
import {NgForOf, NgOptimizedImage} from "@angular/common";
import {Organization} from "../../DataTypes/organization";
import {Request} from "../../DataTypes/request";
import {DataService} from "../../DataTypes/data.service";


@Component({
  selector: 'app-search',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    FormsModule,
    NgForOf,
    NgOptimizedImage
  ],
  templateUrl: './search.component.html',
  styleUrl: './search.component.css'
})
export class SearchComponent {
  // Input-Field (Searching Organization)
  inputValue: string = "";
  // Displayed List of Found Organizations
  orgs: Organization[] = new Array<Organization>();

  constructor(private authService: AuthService){}

  // Input-Change-Event, which searches Organizations
  searchOrganizations(){
    // Resetting GUI
    this.orgs = new Array<Organization>();
    // Searches Organization After Input
    this.authService.searchorganizations(this.inputValue).subscribe(orgs => {
      this.orgs = orgs;
    });
  }

  // Click-Event On Image, that Sends Request to Clicked Organization
  clickedOnOrganizations(org: Organization) {
    if(DataService.user?.id!=undefined && org != undefined){
      // Create Request-Object
      let request: Request = {
        user: DataService.user,
        org: org,
      }

      console.log(request);

      // Sends POST-Request to REST-API, so Request to Organization gets saved
      this.authService.sendRequestToOrganization(request).subscribe(value => {
        // Output for User If worked
        if(value != null) {
          alert("Sent Request successfully!");
        }
        else {
          alert("Something went wrong!");
        }
      });
    }
  }

  protected readonly input = input;
}
