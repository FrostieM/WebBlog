import {HttpClient} from '@angular/common/http';
import {Component, EventEmitter, Input, Output} from '@angular/core';
import { Router } from "@angular/router";

import {ServerService} from "../../shared/services/server.service";
import {IComment} from "../../shared/interfaces/comment.interface";
import {IPost} from "../../shared/interfaces/post.interface";
import {IInfoItem} from "../../shared/interfaces/info-item.interface";
import {CommentViewData} from "../../shared/classes/commentViewData.class";
import {IUser} from "../../shared/interfaces/user.interface";

@Component({
  selector: 'blog-main-post-component',
  host: {
    class: ""
  },
  templateUrl: './main-post.component.html'
})
export class MainPostComponent {

  @Input() mainPost: IInfoItem<IPost>;
  @Input() isCreator: boolean;

  @Output() public messageToUpdate = new EventEmitter();

  public comments: IInfoItem<IComment>[];
  public isCommentFormActive: boolean = false;

  private isCommentOpen: boolean = false;
  public get IsCommentOpen(){
    return this.isCommentOpen;
  }
  public set IsCommentOpen(value: boolean){
    this.isCommentOpen = value;

    if (this.isCommentOpen){
      this.serverService.getComments(this.mainPost.item.id).toPromise().then(response => {
        this.comments = response.map(c => new CommentViewData(c));
      }, error => console.log(error))
    }
  }

  private _comment: IComment = null;

  constructor(private router: Router,
              private http: HttpClient,
              private serverService: ServerService) {

  }

  public updatePosts(){
    this.messageToUpdate.emit();
  }

  public createFullSrc(fileUrl){
    return "https://localhost:5001/" + fileUrl;
  }

  public saveComment(content: string){
    let commentId = this._comment ? this._comment.id : null;
    this.serverService.saveComment(content, this.mainPost.item.id, commentId)
      .subscribe(response => {
        this.IsCommentOpen = true;
      },
          error => console.log(error));

    this.isCommentFormActive = false;
    this._comment = null;
  }

  public cancelAnswer(){
    this.isCommentFormActive=false;
    this._comment = null;
  }

  public answerComment(comment: IComment){
    this._comment = comment;
    console.log(this._comment);
    this.isCommentFormActive = true;
  }


}
