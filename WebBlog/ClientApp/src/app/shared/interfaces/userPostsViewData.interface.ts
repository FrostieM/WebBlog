import {IPagingInfo} from "./pagingInfo.interface";
import {ILikeViewData} from "./likeViewData.interface";
import {IPost} from "./post.interface";

export interface IUserPostsViewData {
  posts: Array<ILikeViewData<IPost>>;
  pagingInfo: IPagingInfo;
}
