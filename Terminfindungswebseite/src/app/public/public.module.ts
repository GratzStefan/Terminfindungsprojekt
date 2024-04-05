import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PublicRoutingModule } from './public-routing.module';
import {LandingModule} from "./landing/landing.module";


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    PublicRoutingModule,
    LandingModule
  ]
})
export class PublicModule { }
