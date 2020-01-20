import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import {CommonModule} from "@angular/common";
import {FormsModule} from "@angular/forms";

import {AuthGuard} from "../shared/services/guards/auth-guard.service";

import {BlogComponent} from "./blog.component";
import {MenuButtonComponent} from "./menu-button/menu-button.component";
import {PostInfoComponent} from "./post-info/post-info.component";
import {ContentComponent} from "./content/content.component";
import {PostCardComponent} from "./post-card/post-card.component";
import {MainPostComponent} from "./main-post/main-post.component";
import {PostFormComponent} from "./post-form/post-form.component";
import {PostPaginationComponent} from "./post-pagination/post-pagination.component";
import {ServerService} from "../shared/services/server.service";

@NgModule({
  declarations: [
    BlogComponent,
    MenuButtonComponent,
    PostInfoComponent,
    ContentComponent,
    MainPostComponent,
    PostCardComponent,
    PostFormComponent,
    PostPaginationComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forRoot([
      {path: '', component: BlogComponent, canActivate: [AuthGuard]},
      {path: ':username', component: BlogComponent, canActivate: [AuthGuard]}
    ]),
    FormsModule
  ],

  providers: [AuthGuard, ServerService],
  exports: []
})
export class BlogModule { }
