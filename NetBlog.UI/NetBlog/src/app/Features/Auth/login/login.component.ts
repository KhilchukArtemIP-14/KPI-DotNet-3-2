import { Component } from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {AuthService} from "../services/auth-service";
import {ToastrService} from "ngx-toastr";
import {Router} from "@angular/router";
import {LoginUserDTO} from "../models/login-user-dto";
import {CookieService} from "ngx-cookie-service";

@Component({
  selector: 'app-login',
  standalone: true,
    imports: [
        ReactiveFormsModule
    ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  loginForm!: FormGroup;
  public constructor(private authService: AuthService,
                     private formBuilder: FormBuilder,
                     private toastr:ToastrService,
                     private router:Router,
                     private cookieService:CookieService,) {
  }

  ngOnInit(): void {
    this.loginForm=this.formBuilder.group({
      email:["", Validators.required],
      password:["", Validators.required],
    })
  }
  submit() {
    var logDTO = this.loginForm.value as LoginUserDTO

    this.authService.login(logDTO).subscribe(data=>{
      console.log(data)

      this.toastr.success("Successfully logged in")
        this.cookieService.set('Authorization', `Bearer ${data.token}`,
          undefined,'/',undefined,true,"Strict");
        this.authService.setUser(data)

        this.router.navigate(['']);
    },
      error => this.toastr.error("Oops, couldn't log in"))
  }

}
