import {UserShortcutDTO} from "../../../Auth/models/UserShortcutDTO";

export interface CommentDTO {
  id:string;
  createdBy: UserShortcutDTO;
  commentText: string;
  dateCreated: Date;
}
