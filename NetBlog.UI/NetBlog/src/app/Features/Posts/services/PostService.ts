import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {CreatePostDTO} from "../models/CreatePostDTO";
import {Observable} from "rxjs";
import {environment} from "../../../../environments/environment";

class UpdatePostDTO {
}

@Injectable({
  providedIn: 'root'
})
export class PostsService {

  constructor(private http: HttpClient) { }

  addPost(dto: CreatePostDTO): Observable<any> {
    return this.http.post<any>(`${environment.apiBaseUrl}/api/Posts`, dto);
  }

  deletePost(id: string): Observable<any> {
    return this.http.delete<any>(`${environment.apiBaseUrl}/api/Posts/${id}`);
  }

  getPostById(id: string): Observable<any> {
    return this.http.get<any>(`${environment.apiBaseUrl}/api/Posts/${id}`);
  }

  getPostSummaries(): Observable<any[]> {
    return this.http.get<any[]>(`${environment.apiBaseUrl}/api/Posts`);
  }

  updatePost(id: string, dto: UpdatePostDTO): Observable<any> {
    return this.http.put<any>(`${environment.apiBaseUrl}/api/Posts/${id}`, dto);
  }
}
