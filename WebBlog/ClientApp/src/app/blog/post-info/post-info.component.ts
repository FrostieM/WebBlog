import {HttpParams} from '@angular/common/http';
import {Component, EventEmitter, Inject, Input, Output} from '@angular/core';
import { Router } from "@angular/router";
import {ServerService} from "../../shared/services/server.service";
import {ILikeViewData} from "../../shared/interfaces/likeViewData.interface";
import {IPost} from "../../shared/interfaces/post.interface";

@Component({
  selector: 'blog-post-info-component',
  host: {
    class: "col-auto ml-auto p-0 m-0"
  },
  templateUrl: './post-info.component.html'
})
export class PostInfoComponent {

  @Input() public isCreator: boolean;
  @Input() public postViewData: ILikeViewData<IPost>;
  @Output() public messageToUpdate = new EventEmitter();


  constructor(private router: Router,
              @Inject("BASE_URL") private baseUrl: string,
              private serverService: ServerService) {

  }

  public deletePost(id: number){
    let params = new HttpParams().set("id", id.toString());

    this.serverService.deletePost(params).subscribe(() => {
      this.messageToUpdate.emit();
    }, err => {
      console.log(err)
    });
  }

  public likePost(id: number){
    let params = new HttpParams().set("postId", id.toString());
    this.postViewData.isLiked = !this.postViewData.isLiked;

    this.serverService.getPost(params).subscribe(response => {
      this.postViewData.isLiked = response.isLiked;
      this.postViewData.likes = response.likes;
    }, error =>
      console.log(error));
  }

}
