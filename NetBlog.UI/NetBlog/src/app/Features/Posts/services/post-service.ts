import {Injectable} from "@angular/core";
import {HttpClient, HttpParams} from "@angular/common/http";
import {CreatePostDTO} from "../models/CreatePostDTO";
import {Observable} from "rxjs";
import {environment} from "../../../../environments/environment";
import {PostDTO} from "../models/PostDTO";
import {PostSummaryDTO} from "../models/PostSummaryDTO";
import {UpdatePostDTO} from "../models/UpdatePostDTO";
import {CommentShortcutDTO} from "../Comments/models/CommentShortcutDTO";
import {PostShortcutDTO} from "../models/PostShortcutDTO";



@Injectable({
  providedIn: 'root'
})
export class PostsService {

  constructor(private http: HttpClient) { }

  addPost(dto: CreatePostDTO): Observable<PostDTO> {
    return this.http.post<PostDTO>(`${environment.apiBaseUrl}/api/Posts`, dto);
  }

  deletePost(id: string): Observable<PostDTO> {
    return this.http.delete<PostDTO>(`${environment.apiBaseUrl}/api/Posts/${id}`);
  }

  getPostById(id: string, commentsToLoad: number = 5): Observable<PostDTO> {
    const params = new HttpParams().set('commentsToLoad', commentsToLoad.toString());
    return this.http.get<PostDTO>(`${environment.apiBaseUrl}/api/Posts/${id}`, { params });
  }

  getPostSummaries(pageNumber: number = 1, pageSize: number = 5, searchQuery: string | null = null, orderByDateAscending: boolean = false): Observable<PostSummaryDTO[]> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString())
      .set('orderByDateAscending', orderByDateAscending.toString());

    if (searchQuery) {
      params = params.set('searchQuery', searchQuery);
    }

    return this.http.get<PostSummaryDTO[]>(`${environment.apiBaseUrl}/api/Posts`, { params });
  }

  updatePost(id: string, dto: UpdatePostDTO): Observable<PostDTO> {
    return this.http.put<PostDTO>(`${environment.apiBaseUrl}/api/Posts/${id}`, dto);
  }

  getShortcutsOfUser(userId: string, pageNumber: number = 1, pageSize: number = 5, orderByDateAscending: boolean = false): Observable<PostShortcutDTO[]> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString())
      .set('orderByDateAscending', orderByDateAscending.toString());

    return this.http.get<PostShortcutDTO[]>(`${environment.apiBaseUrl}/api/Posts/user/${userId}`, { params });
  }
}
