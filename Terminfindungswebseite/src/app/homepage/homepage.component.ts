import { Component, AfterViewInit, ElementRef } from '@angular/core';
import {ɵEmptyOutletComponent, RouterLink, RouterOutlet } from "@angular/router";
import {AuthService, DataService, Organization} from "../auth.service";
import {SearchComponent} from "./search/search.component";
import {OrganizationComponent} from "./organization/organization.component";
import {NgForOf, NgOptimizedImage, NgSwitch, NgSwitchCase} from "@angular/common";
import {CreateComponent} from "./create/create.component";

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
    OrganizationComponent
  ],
  templateUrl: './homepage.component.html',
  styleUrl: './homepage.component.css'
})
export class HomepageComponent {
  type: ComponentType = ComponentType.Default;
  orgs: Organization[] = new Array<Organization>();
  constructor(private elementRef: ElementRef, private authService: AuthService) {}

  ngAfterViewInit(){
    const data = DataService.data;
    if(data != null){
      this.authService.getuserorganizations(data).subscribe(organizations => {
        this.orgs = organizations;
      });
    }
  }
  orgName: string = "";
  clickedOnOrganization(event: MouseEvent){
    const clickedElement = event.target as HTMLElement;
    if (clickedElement.classList.contains('userOrgs')) {
      const value = clickedElement.textContent?.trim();
      if(value != null){
        this.orgName = value;
        this.type = ComponentType.Organization;
      }
    }
  }

  protected readonly ComponentType = ComponentType;
}

export enum ComponentType {
  Default,
  Search,
  Create,
  Organization
}
