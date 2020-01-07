import {PostViewDataInterface} from "./postViewData.interface";
import {PagingInfoInterface} from "./pagingInfo.interface";

export interface UserPostsViewDataInterface {
  posts: Array<PostViewDataInterface>;
  pagingInfo: PagingInfoInterface;
}
