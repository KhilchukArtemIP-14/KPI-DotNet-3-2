import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {CreateCommentDTO} from "../models/CreateCommentDTO";
import {Observable} from "rxjs";
import {environment} from "../../../../../environments/environment";
import {CommentDTO} from "../models/CommentDTO";
import {CommentShortcutDTO} from "../models/CommentShortcutDTO";

@Injectable({
  providedIn: 'root'
})
export class CommentsService {

  constructor(private http: HttpClient) { }

  createComment(dto: CreateCommentDTO): Observable<CommentDTO> {
    return this.http.post<CommentDTO>(`${environment.apiBaseUrl}/api/comments`, dto);
  }

  deleteComment(commentId: string): Observable<CommentDTO | string> {
    return this.http.delete<CommentDTO | string>(`${environment.apiBaseUrl}/api/comments/${commentId}`);
  }

  getCommentsForPost(postId: string): Observable<CommentDTO[]> {
    return this.http.get<CommentDTO[]>(`${environment.apiBaseUrl}/api/comments/post/${postId}`);
  }
  getShortcutsOfUser(id: string): Observable<CommentShortcutDTO[]> {
    return this.http.get<CommentShortcutDTO[]>(`${environment.apiBaseUrl}/api/comments/user/${id}`);
  }
}
