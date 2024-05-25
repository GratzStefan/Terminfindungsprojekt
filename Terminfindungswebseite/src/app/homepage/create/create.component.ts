import { Component } from '@angular/core';
import {NgForOf, NgSwitchCase} from "@angular/common";
import {SearchComponent} from "../search/search.component";
import {FormsModule} from "@angular/forms";
import {AuthService} from "../../auth.service";
import {Organization} from "../../DataTypes/organization";
import {DataService} from "../../DataTypes/data.service";

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
  // Input-Field
  orgName: string = "";

  constructor(private authService: AuthService){}


  createOrganization() {
    // Checks if Input-Field has input
    if(this.orgName!="") {
      // Create Organization-Object
      let org: Organization = {
        name: this.orgName,
        creatorid: DataService.user?.id
      }

      // Request to Create new Organization
      this.authService.createorganization(org).subscribe((response) => {
          alert("Created organization successfully!");
        },
        (error)=> {
          alert("Something went wrong!");
      });
    }
    else {
      alert("Organization must have a name!")
    }
    // Empties Input-Field
    this.orgName = "";
  }
}
