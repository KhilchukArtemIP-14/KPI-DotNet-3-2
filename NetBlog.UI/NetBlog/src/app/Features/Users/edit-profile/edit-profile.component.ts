import {Component, OnInit} from '@angular/core';
import {AuthService} from "../../Auth/services/auth-service";
import {UserSummaryService} from "../Services/user-summary-service";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {ActiveToast, ToastrService} from "ngx-toastr";
import {ActivatedRoute, Router} from "@angular/router";
import {UpdateUserDTO} from "../Models/update-user-dto";
import {UserDto} from "../../Auth/models/user-dto";

@Component({
  selector: 'app-edit-profile',
  standalone: true,
  imports: [
    ReactiveFormsModule
  ],
  templateUrl: './edit-profile.component.html',
  styleUrl: './edit-profile.component.css'
})
export class EditProfileComponent implements OnInit{
  editProfileForm!: FormGroup;
  userId!:string
  loadingData=true;
  public constructor(private toastr:ToastrService,
                     private userSummaryService:UserSummaryService,
                     private fb:FormBuilder,
                     private route:ActivatedRoute,
                     private router:Router,
                     private authService:AuthService) {
  }

  ngOnInit(): void {
    this.userId = this.route.snapshot.params['id'];
    this.userSummaryService.getUserSummary(this.userId).subscribe(data=>{
      console.log(data)
      this.editProfileForm=this.fb.group({
        name:[data.name, Validators.required],
        bio:[data.bio, Validators.required]
      })
      this.loadingData=false;
    })
    }

  Submit() {
    var data = this.editProfileForm.value as UpdateUserDTO
    this.userSummaryService.updateUserSummary(this.userId,data).subscribe(result=>{
      console.log(result)
      var userDTO= {name:result.name,roles:[],userId:""} as UserDto
      this.toastr.success("Changes saved successfully!")
      this.router.navigate(['/users',this.userId])
      this.authService.updateUser(userDTO)
    })
  }
}
