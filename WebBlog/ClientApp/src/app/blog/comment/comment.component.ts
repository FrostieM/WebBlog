import {Component, EventEmitter, Input, Output} from '@angular/core';
import { Router } from "@angular/router";
import {ServerService} from "../../shared/services/server.service";
import {IComment} from "../../shared/interfaces/comment.interface";
import {IInfoItem} from "../../shared/interfaces/info-item.interface";
import {CommentViewData} from "../../shared/classes/commentViewData.class";

@Component({
  selector: 'blog-comment-component',
  host: {
    class: ""
  },
  templateUrl: './comment.component.html'
})
export class CommentComponent{
  @Input() isCreator: boolean;
  @Input() comment: IInfoItem<IComment>;
  @Input() continueBranch: boolean = false;
  @Input() postId: number;

  @Output() messageToUpdate = new EventEmitter();
  @Output() messageToAnswer = new EventEmitter<IComment>();

  private isCommentOpen: boolean = false;
  public get IsCommentOpen(){
    return this.isCommentOpen;
  }
  public set IsCommentOpen(value: boolean){
    this.isCommentOpen = value;
    if (this.isCommentOpen){
      this.getComments();
    }
    else this.comment.item.subComments = null;
  }

  public getComments(){
    this.serverService.getComments(this.postId, this.comment.item.id).toPromise().then(response => {
      this.comment.item.subComments = response.map(c => new CommentViewData(c));
    }, error => console.log(error));
  }

  constructor(private router: Router,
              private serverService: ServerService) {
  }

  public getFullSrc(url: string){
    return "https://localhost:5001/" + url;
  }

  public answerComment(comment: IComment){
    console.log(comment);
    this.messageToAnswer.emit(comment);
  }


}
