import {IUserPostsViewData} from "../interfaces/userPostsViewData.interface";
import {IPagingInfo} from "../interfaces/pagingInfo.interface";
import {IInfoItem} from "../interfaces/info-item.interface";
import {IPost} from "../interfaces/post.interface";
import {PostViewData} from "./postViewData.class";

export class UserPostsViewData implements IUserPostsViewData{
  pagingInfo: IPagingInfo;
  posts: Array<IInfoItem<IPost>>;

  constructor(userPosts: IUserPostsViewData) {
    this.pagingInfo = userPosts.pagingInfo;
    this.posts = userPosts.posts.map(p => new PostViewData(p));
  }
}
