import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import {CommonModule} from "@angular/common";

import {BlogComponent} from "./blog.component";

import {AuthGuard} from "../shared/services/guards/auth-guard.service";
import {MenuButtonComponent} from "./menu-button/menu-button.component";

@NgModule({
  declarations: [
    BlogComponent,
    MenuButtonComponent
  ],
    imports: [
      CommonModule,
      RouterModule.forRoot([
        {path: '', component: BlogComponent, canActivate: [AuthGuard]},
        {path: ':username', component: BlogComponent, canActivate: [AuthGuard]}
      ])
    ],

  providers: [AuthGuard],
  exports: []
})
export class BlogModule { }
