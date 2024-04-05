import { Routes } from '@angular/router';
import {LoginComponent} from "./login/login.component";
import {authGuard} from "./auth.guard";

export const routes: Routes = [
  {
    path: '', loadChildren: () =>
      import('./public/public.module').then((m)=>m.PublicModule),
  },
  {
    path: 'admin',
    loadChildren: () =>
      import('./admin/admin.module').then((m)=>m.AdminModule),
      canActivate: [authGuard],
  },
  {
    path: 'login',
    component: LoginComponent,
  },
];
