import {IPost} from "./post.interface";

export interface IPostViewData {
  post: IPost;
  likes: number;
  isLiked: boolean;
}
