import {User} from "./user";
import {Organization} from "./organization";

export interface Request {
  id?: string;
  user: User;
  org: Organization;
  status?: number;
}
