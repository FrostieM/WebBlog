
import {IPost} from "../interfaces/post.interface";
import {ServerService} from "../services/server.service";
import {EventEmitter} from "@angular/core";
import {IInfoItem} from "../interfaces/info-item.interface";

export class PostViewData implements IInfoItem<IPost>{
  comments: number;
  isLiked: boolean;
  item: IPost;
  likes: number;

  constructor(postInfo: IInfoItem<IPost>) {
    this.comments = postInfo.comments;
    this.isLiked = postInfo.isLiked;
    this.item = postInfo.item;
    this.likes = postInfo.likes;
  }

  public deleteIt(serverService: ServerService, event?: EventEmitter<any>): void {
    serverService.deletePost(this.item.id).subscribe(() => {
      if (event) event.emit();
    }, err => {
      console.log(err)
    });
  }

  public likeIt(serverService: ServerService): void {
    serverService.LikePost(this.item.id).subscribe(response => {
      this.likes = response.likes;
      this.isLiked = response.isLiked;
    }, error =>
      console.log(error));
  }





}
