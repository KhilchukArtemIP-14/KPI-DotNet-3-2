import {Component, Input, OnInit} from '@angular/core';
import {DatePipe, NgForOf, NgIf} from "@angular/common";
import {CommentsService} from "../Services/comments-service";
import {AuthService} from "../../../Auth/services/auth-service";
import {CommentDTO} from "../models/CommentDTO";
import {ToastrService} from "ngx-toastr";
import {FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {CreateCommentDTO} from "../models/CreateCommentDTO";

@Component({
  selector: 'app-view-post-comments',
  standalone: true,
  imports: [
    DatePipe,
    NgForOf,
    NgIf,
    ReactiveFormsModule
  ],
  templateUrl: './view-post-comments.component.html',
  styleUrl: './view-post-comments.component.css'
})
export class ViewPostCommentsComponent implements OnInit{
  public constructor(private commentsService:CommentsService,
                     private authService:AuthService,
                     private toastr:ToastrService,
                     private fb: FormBuilder) {
  }
  @Input() comments!:CommentDTO[]
  @Input() postId!:string

  public commentForm!: FormGroup;
  ngOnInit(): void {
        this.commentForm=this.fb.group({
          comment:["", Validators.required]
        })
    }
    public canDelete(comment:CommentDTO){
      return comment.createdBy.userId==this.authService.getUser()?.userId||
        this.authService.getUser()?.roles.includes("Author");
    }

  delete(comment: CommentDTO) {
    this.commentsService.deleteComment(comment.id).subscribe(data=>{
      this.comments = this.comments.filter(c=>c.id!=comment.id);
      this.toastr.success("Comment removed!")
    })
  }

  submit() {
    var dto = {
      postId: this.postId,
      authorId: this.authService.getUser()?.userId,
      commentText: this.commentForm.controls['comment'].value
    } as CreateCommentDTO
    this.commentsService.createComment(dto).subscribe(data=>{
      this.comments.push(data)
      this.toastr.success("Comment added successfully")
    })
  }
}
