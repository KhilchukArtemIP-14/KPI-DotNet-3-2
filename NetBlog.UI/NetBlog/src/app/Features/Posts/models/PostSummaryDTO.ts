import {UserShortcutDTO} from "../../Auth/models/UserShortcutDTO";

export interface PostSummaryDTO {
  id: string;
  title: string;
  contentPreview: string;
  dateCreated: Date;
  createdBy: UserShortcutDTO;
}
