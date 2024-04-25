import {Component, ElementRef, input, ViewChild} from '@angular/core';
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
    this.orgs = new Array<Organization>();
    this.authService.searchorganizations(this.inputValue).subscribe(orgs => {
      this.orgs = orgs;
    });
  }

  clickedOnOrganizations(event: MouseEvent){
    const clickedElement = event.target as HTMLElement;
    if (clickedElement.classList.contains('containerSearch')) {
      const value = clickedElement.textContent?.trim();

    }
  }

  protected readonly input = input;
}
