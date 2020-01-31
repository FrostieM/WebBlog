import {IPagingInfo} from "./pagingInfo.interface";
import {IPost} from "./post.interface";
import {IInfoItem} from "./info-item.interface";

export interface IUserPostsViewData {
  posts: Array<IInfoItem<IPost>>;
  pagingInfo: IPagingInfo;
}
