import { Component } from '@angular/core';
import {NgForOf, NgSwitchCase} from "@angular/common";
import {SearchComponent} from "../search/search.component";
import {FormsModule} from "@angular/forms";
import {AuthService} from "../../auth.service";

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

  constructor(private authService: AuthService){}
  createOrganization() {
    this.authService.createorganization(this.orgName).subscribe(org => {
      console.log(org);
    });
  }
}
