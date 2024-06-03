import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators} from "@angular/forms";
import {AuthService} from "../services/auth-service";
import {Router} from "@angular/router";
import {RegisterUserDTO} from "../models/register-user-dto";
import {ToastrService} from "ngx-toastr";
import {ConsoleLogger} from "@angular/compiler-cli";

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    FormsModule
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit{
  public registrationForm!:FormGroup;
  public isAuthor = false
  public constructor(private authService: AuthService,
                     private formBuilder: FormBuilder,
                     private toastr:ToastrService,
                     private router:Router) {
  }

  ngOnInit(): void {
        this.registrationForm=this.formBuilder.group({
          name:["", Validators.required],
          email:["", Validators.required],
          password:["", Validators.required],
        })
    }
    public submit(){
      var regDTO = this.registrationForm.value as RegisterUserDTO
      regDTO.roles=[this.isAuthor?"Author":"Reader"]
      console.log(regDTO)
      this.authService.register(regDTO).subscribe(res=>{
        this.toastr.success("Registered successfully")
        this.router.navigate(['/login'])
      },
        error => {this.toastr.error("Oops, couldn't register"); console.log(error)})
    }

  switchIsAuthor() {
    this.isAuthor = !this.isAuthor
  }
}
