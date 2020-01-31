
import {ServerService} from "../services/server.service";
import {EventEmitter} from "@angular/core";
import {IComment} from "../interfaces/comment.interface";
import {IInfoItem} from "../interfaces/info-item.interface";

export class CommentViewData implements IInfoItem<IComment>{
  comments: number;
  isLiked: boolean;
  item: IComment;
  likes: number;

  constructor(comment: IInfoItem<IComment>) {
    this.comments = comment.comments;
    this.isLiked = comment.isLiked;
    this.item = comment.item;
    this.likes = comment.likes;
  }

  public deleteIt(serverService: ServerService, event?: EventEmitter<any>): void {
    serverService.deleteComment(this.item.id).subscribe(() => {
      if (event) event.emit();
    }, err => {
      console.log(err)
    });
  }

  public likeIt(serverService: ServerService): void {
    serverService.LikeComment(this.item.id).subscribe(response => {
      this.likes = response.likes;
      this.isLiked = response.isLiked;
    }, error =>
      console.log(error));
  }



}
