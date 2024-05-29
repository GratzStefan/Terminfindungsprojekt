import {User} from "./user";
import {Organization} from "./organization";
import {StatusType} from "./status.type";

export interface Request {
  id?: string;
  user: User;
  org: Organization;
  status?: StatusType;
}
