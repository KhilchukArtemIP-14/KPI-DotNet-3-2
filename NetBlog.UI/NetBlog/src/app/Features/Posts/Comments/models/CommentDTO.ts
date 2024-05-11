import {UserShortcutDTO} from "../../../Auth/models/UserShortcutDTO";

export interface CommentDTO {
  createdBy: UserShortcutDTO;
  commentText: string;
  dateCreated: Date;
}
