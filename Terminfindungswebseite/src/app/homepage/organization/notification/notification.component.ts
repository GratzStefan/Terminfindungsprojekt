import {Component, Input, SimpleChanges} from '@angular/core';
import {FormsModule} from "@angular/forms";
import {NgForOf, NgClass, NgIf} from "@angular/common";
import {AuthService, Organization, Request, StatusType} from "../../../auth.service";

@Component({
  selector: 'app-notification',
  standalone: true,
  imports: [
    FormsModule,
    NgForOf,
    NgIf,
    NgClass,
  ],
  templateUrl: './notification.component.html',
  styleUrl: './notification.component.css'
})
export class NotificationComponent {
  @Input()
  org: Organization | undefined;
  requests: Request[] = new Array<Request>();

  constructor(private authService: AuthService) {}

  ngOnChanges(changes: SimpleChanges) {
    if(changes['org']) {
      this.requests = new Array<Request>();
      let orgId = this.org?.id;
      if(orgId != null){
        this.authService.getAllRequestsToOrganization(orgId).subscribe(requests => {
          this.requests = requests
        });
      }
    }
  }

  changeStatus(request: Request, type: StatusType) {
    request.status = type;

    this.authService.changeStatus(request).subscribe(data => {
      if(data==1) {
        alert("Changed status successfully!");
        this.requests = this.requests.filter(r => r.id !== request.id);
      }
      else {
        alert("Something failed!");
      }
    });
  }


  protected readonly StatusType = StatusType;
}
