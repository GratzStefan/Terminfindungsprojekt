import {Component, Input} from '@angular/core';

@Component({
  selector: 'app-organization',
  standalone: true,
  imports: [],
  templateUrl: './organization.component.html',
  styleUrl: './organization.component.css'
})
export class OrganizationComponent {
  @Input()
  orgName!: string;
}
