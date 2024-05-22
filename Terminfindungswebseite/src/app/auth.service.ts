import {Injectable} from '@angular/core';
import {Router} from "@angular/router";
import {HttpClient} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})

export class AuthService {
  private apiUrl = 'http://localhost:8080/api/';
  session: any;

  constructor(private router: Router, private http: HttpClient) {
    let session: any = localStorage.getItem('session');

    if(session) {
      session = JSON.parse(session);
    }

    this.session = session;
  }

  login(username: string, password: string){
    let user = this.http.get<User>(`${this.apiUrl}user/login?username=${username}&password=${password}`);
    this.session = user;
    localStorage.setItem('session', JSON.stringify(this.session));
    return user;
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

  sendRequestToOrganization(request: Request) {
    return this.http.post(`${this.apiUrl}request/send`, request, {responseType: "json"});
  }

  getAllRequestsToOrganization(orgid: string) {
    return this.http.get<Request[]>(`${this.apiUrl}request/findToOrganization/${orgid}`);
  }

  changeStatus(request: Request){
    return this.http.put(`${this.apiUrl}request/changeStatus?adminid=${DataService.user?.id}`, request);
  }

  getEventsOfUser() {
    return this.http.get<Event[]>(`${this.apiUrl}events/find/${DataService.user?.id}`);
  }

  getAllRequestsOfUser() {
    return this.http.get<Request[]>(`${this.apiUrl}request/findOfUser/${DataService.user?.id}`);
  }

  changeUser(user: User){
    return this.http.put(`${this.apiUrl}user/modifyUser`, user);
  }

  deleteOrganization(org: Organization){
    return this.http.delete(`${this.apiUrl}organization/delete/${org.id}`);
  }

  logout(){
    this.session = undefined;
    localStorage.removeItem('session');
    this.router.navigateByUrl('/')
  }
}

export interface User {
  id?: string;
  firstname: string;
  lastname: string;
  username: string;
  password?: string;
}

export interface Organization {
  id?: string;
  name: string;
  creatorid?: string;
}

export interface Event {
  id?: string;
  titel: string;
  description?: string;
  datetimestart: Date;
  datetimeend: Date;
  organizationid: string;
}

export interface Request {
  id?: string;
  user: User;
  org: Organization;
  status?: number;
}

@Injectable({
  providedIn: 'root'
})
export class DataService {
  static user: User;
}

export enum StatusType{
  WAITING,
  REJECTED,
  ACCEPTED,
}

export interface GroupedEvent {
  date: Date;
  events: any[];
}
