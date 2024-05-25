import {Injectable} from '@angular/core';
import {Router} from "@angular/router";
import {HttpClient} from "@angular/common/http";
import {User} from "./DataTypes/user";
import {DataService} from "./DataTypes/data.service";
import {Organization} from "./DataTypes/organization";
import {Request} from "./DataTypes/request";
import {Event} from "./DataTypes/event";

@Injectable({
  providedIn: 'root'
})

export class AuthService {
  // API-URL
  private apiUrl = 'http://localhost:8080/api/';
  // SESSION (User)
  session: User|undefined;

  constructor(private router: Router, private http: HttpClient) {
    // Finds If He Already Logged In
    let session = localStorage.getItem('session');

    if(session) {
      DataService.user = JSON.parse(session);
      this.session = DataService.user;
    }
  }


  // Users-Operation

  login(username: string, password: string){
    this.http.get<User>(`${this.apiUrl}user/login?username=${username}&password=${password}`).subscribe(value => {
      this.session = value;
      DataService.user = value;
      localStorage.setItem('session', JSON.stringify(value));
      this.router.navigateByUrl('/homepage');
      },
    error => {
      alert('Invalid username or password');
    });

    return false;
  }

  signup(firstname: string, lastname: string, username: string, password: string){
    let u: User = {
      firstname: firstname,
      lastname: lastname,
      username: username,
      password: password
    }
    return this.http.post<User>(`${this.apiUrl}user/signup`, u);
  }

  changeUser(user: User){
    return this.http.put(`${this.apiUrl}user/modifyUser`, user);
  }

  logout(){
    this.session = undefined;
    localStorage.removeItem('session');
    this.router.navigateByUrl('/')
  }


  // Organization-Operations

  getuserorganizations(userid: string){
    return this.http.get<Organization[]>(`${this.apiUrl}organization/searchOrganizations/${userid}`);
  }

  createorganization(org: Organization){
    return this.http.post(`${this.apiUrl}organization/create`, org, {responseType: "text"});
  }

  searchorganizations(name: string) {
    return this.http.get<Organization[]>(`${this.apiUrl}organization/search/${name}`);
  }

  geteventsorganization(orgid: string){
    return this.http.get<Event[]>(`${this.apiUrl}events/search/${orgid}`);
  }

  getuserlist(orgid: string | undefined) {
    return this.http.get<User[]>(`${this.apiUrl}organization/userListOrganization/${orgid}`);
  }

  addEvent(event: Event){
    return this.http.post(`${this.apiUrl}events/add`, event, {responseType: "text"})
  }

  promoteUser(userid: string | undefined, orgid: string | undefined, adminid: string | undefined) {
    return this.http.put(`${this.apiUrl}organization/promote?userid=${userid}&orgid=${orgid}&adminid=${adminid}`, null);
  }

  removeuserorganization(userid: string | undefined, orgid: string | undefined, adminid: string | undefined) {
    return this.http.delete(`${this.apiUrl}organization/removeUser?userid=${userid}&orgid=${orgid}&adminid=${adminid}`);
  }

  getAllRequestsToOrganization(orgid: string) {
    return this.http.get<Request[]>(`${this.apiUrl}request/findToOrganization/${orgid}`);
  }

  changeStatus(request: Request){
    return this.http.put(`${this.apiUrl}request/changeStatus?adminid=${DataService.user?.id}`, request);
  }

  deleteOrganization(org: Organization){
    return this.http.delete(`${this.apiUrl}organization/delete/${org.id}`);
  }


  // Request

  sendRequestToOrganization(request: Request) {
    return this.http.post(`${this.apiUrl}request/send`, request, {responseType: "json"});
  }

  getAllRequestsOfUser() {
    return this.http.get<Request[]>(`${this.apiUrl}request/findOfUser/${DataService.user?.id}`);
  }


  // Events

  getEventsOfUser() {
    return this.http.get<Event[]>(`${this.apiUrl}events/find/${DataService.user?.id}`);
  }
}
