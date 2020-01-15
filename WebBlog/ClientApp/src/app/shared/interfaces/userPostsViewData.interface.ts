import {IPagingInfo} from "./pagingInfo.interface";
import {IPostViewData} from "./postViewData.interface";

export interface IUserPostsViewData {
  posts: Array<IPostViewData>;
  pagingInfo: IPagingInfo;
}
