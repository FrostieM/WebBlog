import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import {CommonModule} from "@angular/common";
import {FormsModule} from "@angular/forms";

import {BlogComponent} from "./blog.component";
import {MenuButtonComponent} from "./menu-button/menu-button.component";
import {ContentComponent} from "./content/content.component";
import {PostCardComponent} from "./post-card/post-card.component";
import {MainPostComponent} from "./main-post/main-post.component";
import {PostFormComponent} from "./post-form/post-form.component";
import {PostPaginationComponent} from "./post-pagination/post-pagination.component";
import {ServerService} from "../shared/services/server.service";
import {AuthGuard} from "../shared/guards/auth.guard";
import {CreatedPipe} from "../shared/pipes/created.pipe";
import {CommentComponent} from "./comment/comment.component";
import {ItemInfoComponent} from "./item-info/item-info.component";

@NgModule({
  declarations: [
    BlogComponent,
    MenuButtonComponent,
    ItemInfoComponent,
    ContentComponent,
    MainPostComponent,
    PostCardComponent,
    PostFormComponent,
    PostPaginationComponent,
    CommentComponent,
    CreatedPipe
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
