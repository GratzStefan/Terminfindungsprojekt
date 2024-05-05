import {Component, ElementRef} from '@angular/core';
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
  styleUrl: './homepage.component.css',
  //TODO: Delete afterwards (for testing purposes)
  host: {ngSkipHydration: 'true'},
})
export class HomepageComponent {
  type: ComponentType = ComponentType.Default;
  orgs: Organization[] = new Array<Organization>();
  constructor(private elementRef: ElementRef, private authService: AuthService) {}

  ngAfterViewInit(){
    let data = DataService.data;

    //TODO: Delete afterwards (Due to testing purposes)
    data = "663519a065014269ff6d96ac";

    if(data != null && data != ""){
      this.authService.getuserorganizations(data).subscribe(organizations => {
        this.orgs = organizations;
      });
    }
  }
  org: Organization | undefined;
  clickedOnOrganization(event: MouseEvent){
    const clickedElement = event.target as HTMLElement;
    if (clickedElement.classList.contains('userOrgs')) {
      const value = clickedElement.textContent?.trim();
      if(value != null){
        this.org = this.orgs.find(o => o.name === value);
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
