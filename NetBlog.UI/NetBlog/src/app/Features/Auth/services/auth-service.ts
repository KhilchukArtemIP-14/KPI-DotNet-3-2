import {UserDto} from "../models/user-dto";
import {HttpClient} from "@angular/common/http";
import {BehaviorSubject, Observable} from "rxjs";
import {Injectable} from "@angular/core";
import {RegisterUserDTO} from "../models/register-user-dto";
import {environment} from "../../../../environments/environment";
import {LoginUserDTO} from "../models/login-user-dto";
import {LoginResponseDTO} from "../models/login-response-dto";
import {CookieService} from "ngx-cookie-service";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  $user = new BehaviorSubject<UserDto | undefined>(undefined);

  constructor(private http: HttpClient,
              private cookieService:CookieService) { }

  register(user: RegisterUserDTO): Observable<any> {
    return this.http.post(`${environment.apiBaseUrl}/api/Auth/register`, user);
  }

  login(user: LoginUserDTO): Observable<LoginResponseDTO> {
    return this.http.post<LoginResponseDTO>(`${environment.apiBaseUrl}/api/Auth/login`, user);
  }
  setUser(response:LoginResponseDTO){
    this.$user.next(response)
    localStorage.setItem('Name', response.name)
    localStorage.setItem('Roles', response.roles.join(','))
    localStorage.setItem('Email', response.email)
    localStorage.setItem('UserId', response.userId)
  }
  user() : Observable<UserDto | undefined> {
    return this.$user.asObservable();
  }

  getUser():UserDto|undefined{
    const email = localStorage.getItem('Email');
    const roles = localStorage.getItem('Roles');
    const name = localStorage.getItem('Name');
    const id = localStorage.getItem('UserId');
    if (email && roles && name && id ) {
      return {
        email: email,
        userId:id,
        name: name,
        roles: roles.split(','),
      };
    }
    return undefined;
  }
  logout() {
    this.$user.next(undefined);
    localStorage.clear();
    this.cookieService.delete('Authorization', '/');
  }
  updateUser(summary:UserDto){
    localStorage.setItem('Name', summary.name)
    localStorage.setItem('userId', summary.userId)
    this.$user.next(this.getUser())
  }
}
