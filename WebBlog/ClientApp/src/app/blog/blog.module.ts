import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import {CommonModule} from "@angular/common";

import {BlogComponent} from "./blog.component";

import {AuthGuard} from "../shared/services/guards/auth-guard.service";
import {MenuButtonComponent} from "./menu-button/menu-button.component";
import {PostInfoComponent} from "./post-info/post-info.component";
import {ContentComponent} from "./content/content.component";
import {FormsModule} from "@angular/forms";
import {PostCardComponent} from "./post-card/post-card.component";

@NgModule({
  declarations: [
    BlogComponent,
    MenuButtonComponent,
    PostInfoComponent,
    ContentComponent,
    PostCardComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forRoot([
      {path: '', component: BlogComponent, canActivate: [AuthGuard]},
      {path: ':username', component: BlogComponent, canActivate: [AuthGuard]}
    ]),
    FormsModule
  ],

  providers: [AuthGuard],
  exports: []
})
export class BlogModule { }
