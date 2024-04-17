import {CanActivateFn, Router} from '@angular/router';
import {inject} from "@angular/core";
import {AuthService} from "./auth.service";

export const authGuard: CanActivateFn = (route, state) => {
  if(inject(AuthService)) return true;
  inject(Router).navigateByUrl('/');
  return false;
};
