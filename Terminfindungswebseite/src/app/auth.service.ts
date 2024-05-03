import {Injectable} from '@angular/core';
import {Router} from "@angular/router";
import {HttpClient} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})

export class AuthService {
  private apiUrl = 'http://localhost:8080/api/';
  //session: any;
  constructor(private router: Router, private http: HttpClient) {
    /*let session: any = localStorage.getItem('session');
    if(session) {
      session = JSON.parse(session);
    }

    this.session = session;*/
  }

  login(username: string, password: string){
    /*if(user){
      this.session = user;
      localStorage.setItem('session', JSON.stringify(this.session));
    }*/

    return this.http.get<User>(`${this.apiUrl}user/login?username=${username}&password=${password}`);
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

  createorganization(orgName: string){
    let org: Organization = {
      name: orgName,
      creatorid: DataService.data
    }
    console.log(org.id, org.name, org.creatorid);

    return this.http.post<string>(`${this.apiUrl}organization/create`, org);
  }

  searchorganizations(name: string) {
    return this.http.get<Organization[]>(`${this.apiUrl}organization/search/${name}`);
  }

  geteventsorganization(orgid: string){
    return this.http.get<Event[]>(`${this.apiUrl}events/search/${orgid}`);
  }

  getuserlist(orgid: string) {
    return this.http.get<User[]>(`${this.apiUrl}organization/userListOrganization/${orgid}`);
  }

  promoteUser(userid: string | undefined, orgid: string | undefined, adminid: string | undefined) {
    return this.http.put(`${this.apiUrl}organization/promote?userid=${userid}&orgid=${orgid}&adminid=${adminid}`, null);
  }

  removeuserorganization(userid: string | undefined, orgid: string | undefined, adminid: string | undefined) {
    return this.http.delete(`${this.apiUrl}organization/removeUser?userid=${userid}&orgid=${orgid}&adminid=${adminid}`);
  }

  logout(){
    //this.session = undefined;
    //localStorage.removeItem('session');
    this.router.navigateByUrl('/')
  }
}

export interface User {
  id?: string;
  firstname: string;
  lastname: string;
  username: string;
  password: string;
}

export interface Organization {
  id?: string;
  name: string;
  creatorid?: string;
}

export interface Event {
  id?: string;
  titel: string;
  description: string;
  datetimestart: Date;
  datetimeend: Date;
  organizationid: string;
}
@Injectable({
  providedIn: 'root'
})
export class DataService {
  static data: string | undefined = "";
}
