import {PostInterface} from "./post.interface";

export interface PostViewDataInterface {
  post: PostInterface;
  likes: number;
  isLiked: boolean;
}
