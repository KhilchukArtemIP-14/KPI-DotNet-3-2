import {Injectable} from "@angular/core";
import {HttpClient, HttpParams} from "@angular/common/http";
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

  getCommentsForPost(postId: string, pageNumber: number = 1, pageSize: number = 5, orderByDateAscending: boolean = false): Observable<CommentDTO[]> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString())
      .set('orderByDateAscending', orderByDateAscending.toString());

    return this.http.get<CommentDTO[]>(`${environment.apiBaseUrl}/api/comments/post/${postId}`, { params });
  }

  getShortcutsOfUser(userId: string, pageNumber: number = 1, pageSize: number = 5, orderByDateAscending: boolean = false): Observable<CommentShortcutDTO[]> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString())
      .set('orderByDateAscending', orderByDateAscending.toString());

    return this.http.get<CommentShortcutDTO[]>(`${environment.apiBaseUrl}/api/comments/user/${userId}`, { params });
  }
}
