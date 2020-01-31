import {IUser} from "./user.interface";
import {IInfoItem} from "./info-item.interface";

export interface IComment {
  id: number;
  user: IUser;
  content: string;
  subComments: IInfoItem<IComment>[];
  created: string;
}
