import {ServerService} from "../services/server.service";
import {EventEmitter} from "@angular/core";

export interface IInfoItem<T>{
  item: T
  likes: number;
  isLiked: boolean;
  comments: number;

  likeIt(serverService: ServerService): void;
  deleteIt(serverService: ServerService, event?: EventEmitter<any>): void;
}
