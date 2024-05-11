import {UserShortcutDTO} from "../../Auth/models/UserShortcutDTO";
import {CommentDTO} from "../Comments/models/CommentDTO";

export interface PostDTO {
  id: string;
  title: string;
  content: string;
  dateCreated: Date;
  dateUpdated?: Date;
  createdBy: UserShortcutDTO;
  comments: CommentDTO[];
}
