import {UserShortcutDTO} from "../../../Auth/models/UserShortcutDTO";

export interface CreateCommentDTO{
  postId:string;
  authorId: string  ;
  commentText: string;
}
