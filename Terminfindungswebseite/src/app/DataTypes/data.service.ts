import {Injectable} from "@angular/core";
import {User} from "./user";

@Injectable({
  providedIn: 'root'
})
export class DataService {
  static user: User;
}
