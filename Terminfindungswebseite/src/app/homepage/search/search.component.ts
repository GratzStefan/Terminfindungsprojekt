import {Component, ElementRef, ViewChild} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {AuthService, Organization} from "../../auth.service";
import {NgForOf} from "@angular/common";

@Component({
  selector: 'app-search',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    FormsModule,
    NgForOf
  ],
  templateUrl: './search.component.html',
  styleUrl: './search.component.css'
})
export class SearchComponent {
  @ViewChild('orgList') orgList!: ElementRef;
  inputValue: string = "";
  orgs: Organization[] = new Array<Organization>();
  constructor(private authService: AuthService){}
  searchOrganizations(){
    //this.removeAllChildren();
    this.authService.searchorganizations(this.inputValue).subscribe(org => {
      this.orgs = org;
    });
    /*this.authService.searchorganizations(this.inputValue).subscribe(orgs => {
      orgs.forEach(org => {
        this.addContainer(org);
      })
    });*/
  }

  removeAllChildren() {
    while(this.orgList.nativeElement.firstChild){
      this.orgList.nativeElement.removeChild(this.orgList.nativeElement.firstChild);
    }
  }
  addContainer(org: Organization) {
    var newContainer = document.createElement("div");
    newContainer.className = 'containerSearch';
    var containerContent = document.createTextNode(org.name);
    newContainer.appendChild(containerContent);
    this.orgList.nativeElement.appendChild(newContainer);
  }
}
