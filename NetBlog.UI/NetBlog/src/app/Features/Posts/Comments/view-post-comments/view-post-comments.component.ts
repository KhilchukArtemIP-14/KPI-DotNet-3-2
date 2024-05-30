import {Component, HostListener, Input, OnInit} from '@angular/core';
import {DatePipe, NgForOf, NgIf} from "@angular/common";
import {CommentsService} from "../Services/comments-service";
import {AuthService} from "../../../Auth/services/auth-service";
import {CommentDTO} from "../models/CommentDTO";
import {ToastrService} from "ngx-toastr";
import {FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {CreateCommentDTO} from "../models/CreateCommentDTO";
import {RouterLink} from "@angular/router";

@Component({
  selector: 'app-view-post-comments',
  standalone: true,
  imports: [
    DatePipe,
    NgForOf,
    NgIf,
    ReactiveFormsModule,
    RouterLink
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
  private pageSize!:number;
  private pageNumber = 1;
  public isLoading = false;
  public hasMore = true;
  private orderByDateAscending: boolean = false;

  public commentForm!: FormGroup;
  ngOnInit(): void {
    this.pageSize=this.comments.length;
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
    this.commentForm.controls['comment'].reset()
    this.commentsService.createComment(dto).subscribe(data=>{
      this.comments.unshift(data)
      this.toastr.success("Comment added successfully")
    })
  }
  @HostListener('window:scroll', [])
  onScroll(): void {
    const position = window.scrollY + window.innerHeight;
    const height = document.documentElement.scrollHeight;
    if (!this.isLoading && this.hasMore && position >= height) {
      this.pageNumber++;
      this.isLoading=true;
      this.commentsService
        .getCommentsForPost(this.postId,this.pageNumber,this.pageSize,this.orderByDateAscending)
        .subscribe(data=>{
        this.hasMore = data.length!=0;
        this.comments = this.comments.concat(data);
        this.isLoading=false;
      })
    }
  }

  onOrderChange(b: boolean) {
    this.orderByDateAscending = b;
    this.pageNumber=1;
    this.isLoading=true;
    this.hasMore = true;
    this.commentsService.getCommentsForPost(this.postId,this.pageNumber,this.pageSize,this.orderByDateAscending).subscribe(data=>{
      this.comments=data;
      console.log(data)
      this.isLoading=false;
      this.hasMore=data.length!=0;
    })
  }
}
