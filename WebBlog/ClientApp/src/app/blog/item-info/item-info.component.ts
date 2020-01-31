import {Component, EventEmitter, Input, Output} from '@angular/core';

import {ServerService} from "../../shared/services/server.service";
import {IInfoItem} from "../../shared/interfaces/info-item.interface";
import {IComment} from "../../shared/interfaces/comment.interface";
import {IPost} from "../../shared/interfaces/post.interface";

@Component({
  selector: 'blog-item-info-component',
  host: {
    class: "col-auto ml-auto p-0 m-0"
  },
  templateUrl: './item-info.component.html'
})
export class ItemInfoComponent {

  @Input() public isCreator: boolean;
  @Input() public isComment: boolean = false;

  private _itemInfo: IInfoItem<IComment | IPost> = null;
  @Input() public set ItemInfo(itemInfo: IInfoItem<IComment | IPost>){
    this._itemInfo = itemInfo;
  };

  @Input() public isOpenComments: boolean = false;
  @Output() public isOpenCommentsChange = new EventEmitter;

  @Output() public messageToUpdate = new EventEmitter();

  constructor(private serverService: ServerService) {}

  public deleteItem(){
    this._itemInfo.deleteIt(this.serverService, this.messageToUpdate);
  }

  public changeDisplayComments(){
    this.isOpenComments = !this.isOpenComments;
    this.isOpenCommentsChange.emit(this.isOpenComments);
  }

  public likeItem(){
    this._itemInfo.isLiked = !this._itemInfo.isLiked;
    this._itemInfo.likeIt(this.serverService,);
  }

  public update(){
    this.messageToUpdate.emit();
  }

}
