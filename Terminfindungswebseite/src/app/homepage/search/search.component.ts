import {Component, ElementRef, input, ViewChild} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {AuthService, DataService, Organization, Request} from "../../auth.service";
import {NgForOf, NgOptimizedImage} from "@angular/common";


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
  inputValue: string = "";
  orgs: Organization[] = new Array<Organization>();

  constructor(private authService: AuthService){}
  searchOrganizations(){
    this.orgs = new Array<Organization>();
    this.authService.searchorganizations(this.inputValue).subscribe(orgs => {
      this.orgs = orgs;
    });
  }

  clickedOnOrganizations(org: Organization) {
    if(DataService.user?.id!=undefined && org != undefined){
      let request: Request = {
        user: DataService.user,
        org: org,
      }

      this.authService.sendRequestToOrganization(request).subscribe(value => {
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
