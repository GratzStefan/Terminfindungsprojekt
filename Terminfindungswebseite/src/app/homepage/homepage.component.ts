import {Component, ElementRef} from '@angular/core';
import {ɵEmptyOutletComponent, RouterLink, RouterOutlet } from "@angular/router";
import {AuthService, DataService, Organization, User} from "../auth.service";
import {SearchComponent} from "./search/search.component";
import {OrganizationComponent} from "./organization/organization.component";
import {NgForOf, NgOptimizedImage, NgSwitch, NgSwitchCase} from "@angular/common";
import {CreateComponent} from "./create/create.component";
import {HomeComponent} from "./home/home.component";
import {UserComponent} from "./user/user.component";

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
  //TODO: Delete afterwards (for testing purposes)
  host: {ngSkipHydration: 'true'},
})
export class HomepageComponent {
  type: ComponentType = ComponentType.Default;
  orgs: Organization[] = new Array<Organization>();
  constructor(private elementRef: ElementRef, protected authService: AuthService) {}

  ngAfterViewInit(){
    //let data = DataService.user;

    //TODO: Delete afterwards (Due to testing purposes)
    let data: User = {
      firstname: "Stefan", lastname: "Gratz",
      id: "663519a065014269ff6d96ac",
      username: "test"
    };
    DataService.user = data;

    if(data.id != undefined && data.id != "") {
      this.authService.getuserorganizations(data?.id).subscribe(organizations => this.orgs = organizations);
    }
  }

  curOrg: Organization | undefined;
  clickedOnOrganization(event: MouseEvent){
    const clickedElement = event.target as HTMLElement;
    if (clickedElement.classList.contains('userOrgs')) {
      const value = clickedElement.textContent?.trim();
      if(value != null){
        this.curOrg = this.orgs.find(o => o.name === value);
        this.type = ComponentType.Organization;
      }
    }
  }

  protected readonly ComponentType = ComponentType;
  protected readonly DataService = DataService;
}

export enum ComponentType {
  Default,
  Search,
  Create,
  Organization,
  User
}
