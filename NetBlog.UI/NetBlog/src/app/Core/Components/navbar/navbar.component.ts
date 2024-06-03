import {Component, OnInit} from '@angular/core';
import {UserDto} from "../../../Features/Auth/models/user-dto";
import {AuthService} from "../../../Features/Auth/services/auth-service";
import {Router, RouterLink} from "@angular/router";
import {NgIf} from "@angular/common";

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [
    NgIf,
    RouterLink
  ],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit{
  user?: UserDto;

  constructor(private authService: AuthService,
              private router: Router) {
  }
  ngOnInit(): void {
    this.authService.user()
      .subscribe({
        next: (response) => {
          this.user = response;
        }
      });

    this.user = this.authService.getUser();
  }

  logout() {
    this.authService.logout();
  }
}
