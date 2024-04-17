import { Routes } from '@angular/router';
import {LoginComponent} from "./login/login.component";
import {SignupComponent} from "./signup/signup.component";
import {HomepageComponent} from "./homepage/homepage.component";

export const routes: Routes = [
  {
    path: '', loadChildren: () =>
      import('./public/public.module').then((m)=>m.PublicModule),
  },
  {
    path: 'homepage',
    component: HomepageComponent
  },
  {
    path: 'login',
    component: LoginComponent,
  },
  {
    path: 'signup',
    component: SignupComponent,
  },
];
