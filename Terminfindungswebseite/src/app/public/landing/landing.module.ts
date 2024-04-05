import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {LandingComponent} from "./landing.component";
import {RouterModule} from "@angular/router";



@NgModule({
  declarations: [],
  exports: [LandingComponent],
  imports: [
    CommonModule,
    LandingComponent,
    RouterModule
  ]
})
export class LandingModule { }
