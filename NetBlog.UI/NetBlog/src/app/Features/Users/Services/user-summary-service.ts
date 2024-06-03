import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {UpdateUserDTO} from "../Models/update-user-dto";
import {UserSummaryDTO} from "../Models/user-summary-dto";
import {environment} from "../../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class UserSummaryService {

  constructor(private http: HttpClient) { }

  getUserSummary(id: string): Observable<UserSummaryDTO> {
    return this.http.get<UserSummaryDTO>(`${environment.apiBaseUrl}/api/UserSummary/${id}`);
  }

  updateUserSummary(id: string, dto: UpdateUserDTO): Observable<UserSummaryDTO > {
    return this.http.put<UserSummaryDTO>(`${environment.apiBaseUrl}/api/UserSummary/${id}`, dto);
  }
}
