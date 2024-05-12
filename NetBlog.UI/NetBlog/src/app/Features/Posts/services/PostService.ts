import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {CreatePostDTO} from "../models/CreatePostDTO";
import {Observable} from "rxjs";
import {environment} from "../../../../environments/environment";
import {PostDTO} from "../models/PostDTO";
import {PostSummaryDTO} from "../models/PostSummaryDTO";
import {UpdatePostDTO} from "../models/UpdatePostDTO";



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

  getPostById(id: string): Observable<PostDTO> {
    return this.http.get<PostDTO>(`${environment.apiBaseUrl}/api/Posts/${id}`);
  }

  getPostSummaries(): Observable<PostSummaryDTO[]> {
    return this.http.get<PostSummaryDTO[]>(`${environment.apiBaseUrl}/api/Posts`);
  }

  updatePost(id: string, dto: UpdatePostDTO): Observable<PostDTO> {
    return this.http.put<PostDTO>(`${environment.apiBaseUrl}/api/Posts/${id}`, dto);
  }
}
