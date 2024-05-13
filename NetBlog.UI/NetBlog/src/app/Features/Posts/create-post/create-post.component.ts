import {Component, OnInit} from '@angular/core';
import {PostsService} from "../services/PostService";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {CreatePostDTO} from "../models/CreatePostDTO";
import {Router} from "@angular/router";
import {ToastrService} from "ngx-toastr";
import {AuthService} from "../../Auth/services/auth-service";

@Component({
  selector: 'app-create-post',
  standalone: true,
  imports: [
    ReactiveFormsModule
  ],
  templateUrl: './create-post.component.html',
  styleUrl: './create-post.component.css'
})
export class CreatePostComponent implements OnInit{
  public postForm!:FormGroup;
  public constructor(private postService:PostsService,
                     private fb: FormBuilder,
                     private router: Router,
                     private toastr:ToastrService,
                     private authService:AuthService
) {
  }

  ngOnInit(): void {
        this.postForm=this.fb.group({
          title:["", Validators.required],
          contentPreview:["", Validators.required],
          content:["",Validators.required],
          createdBy:[this.authService.getUser()?.userId, Validators.required]
        })
    }
  public Submit(){
    var data = this.postForm.value as CreatePostDTO;

    this.postService.addPost(data).subscribe(()=>{
      this.toastr.success("Post created successfully!")
      this.router.navigate(['/posts']);
    },
      error => this.toastr.error("Oops, couldn't create a post"))
  }
}
