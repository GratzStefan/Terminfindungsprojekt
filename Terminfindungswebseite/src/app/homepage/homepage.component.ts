import { Component, AfterViewInit, ElementRef } from '@angular/core';
import {RouterLink, RouterOutlet, ɵEmptyOutletComponent} from "@angular/router";
import {AuthService, DataService, Organization} from "../auth.service";
import {SearchComponent} from "./search/search.component";
import {NgSwitch, NgSwitchCase} from "@angular/common";
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
    CreateComponent
  ],
  templateUrl: './homepage.component.html',
  styleUrl: './homepage.component.css'
})
export class HomepageComponent {
  type: ComponentType = ComponentType.Default;
  constructor(private elementRef: ElementRef, private authService: AuthService, private dataService: DataService) {}

  ngAfterViewInit(){
    var data = this.dataService.data;
    if(typeof data === "string"){
      this.authService.getuserorganizations(data).subscribe(organizations => {
        organizations.forEach(org => {
          this.addContainer(org.name);
        });
      });
    }
  }

  addContainer(org: string | undefined) {
    if (typeof org === "string") {
      var newContainer = document.createElement("div");
      newContainer.className = "container";
      var containerContent = document.createTextNode(org);
      newContainer.appendChild(containerContent);
      var containerList = document.getElementById("containerList");
      containerList?.appendChild(newContainer);
    }
  }

  protected readonly ComponentType = ComponentType;
}

export enum ComponentType {
  Default,
  Search,
  Create
}
