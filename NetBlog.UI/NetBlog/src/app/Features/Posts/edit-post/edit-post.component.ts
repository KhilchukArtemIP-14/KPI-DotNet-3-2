import { Component } from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {PostsService} from "../services/post-service";
import {ActivatedRoute, Router} from "@angular/router";
import {ToastrService} from "ngx-toastr";
import {routes} from "../../../app.routes";
import {AuthService} from "../../Auth/services/auth-service";
import {UpdatePostDTO} from "../models/UpdatePostDTO";

@Component({
  selector: 'app-edit-post',
  standalone: true,
    imports: [
        ReactiveFormsModule
    ],
  templateUrl: './edit-post.component.html',
  styleUrl: './edit-post.component.css'
})
export class EditPostComponent {
  public editPostForm!:FormGroup;
  public postId!:string;
  public loadingData =true;


  public constructor(private postService:PostsService,
                     private fb: FormBuilder,
                     private router: Router,
                     private toastr:ToastrService,
                     private route: ActivatedRoute,
  ) {
  }

  ngOnInit(): void {
    this.postId = this.route.snapshot.params['id'];
    this.postService.getPostById(this.postId).subscribe(data=>{
      this.editPostForm=this.fb.group({
        title:[data.title, Validators.required],
        contentPreview:[data.contentPreview, Validators.required],
        content:[data.content,Validators.required],
      })
      this.loadingData=false;
    })

  }
  Submit() {
    var editDTO = this.editPostForm.value as UpdatePostDTO

    this.postService.updatePost(this.postId,editDTO).subscribe(data=>{
      this.toastr.success("Changes saved successfully")
      this.router.navigate(['/posts/',this.postId])
    },
      error => this.toastr.error("Oops, couldn't save changes")
    )
  }
}
