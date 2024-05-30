import {Component, OnInit} from '@angular/core';
import {UserSummaryService} from "../Services/user-summary-service";
import {AuthService} from "../../Auth/services/auth-service";
import {ActivatedRoute, Router, RouterLink} from "@angular/router";
import {UserSummaryDTO} from "../Models/user-summary-dto";
import {ToastrService} from "ngx-toastr";
import {DatePipe, NgForOf, NgIf} from "@angular/common";
import {PostsService} from "../../Posts/services/post-service";
import {CommentsService} from "../../Posts/Comments/Services/comments-service";
import {CommentShortcutDTO} from "../../Posts/Comments/models/CommentShortcutDTO";
import {PostShortcutDTO} from "../../Posts/models/PostShortcutDTO";

@Component({
  selector: 'app-view-profile',
  standalone: true,
  imports: [
    NgForOf,
    NgIf,
    RouterLink,
    DatePipe
  ],
  templateUrl: './view-profile.component.html',
  styleUrl: './view-profile.component.css'
})
export class ViewProfileComponent implements OnInit{
  public constructor(private userSummaryService:UserSummaryService,
                     private postsService:PostsService,
                     private commentsService:CommentsService,
                     private authService:AuthService,
                     private route:ActivatedRoute,
                     private router:Router,
                     private toastr:ToastrService) {
  }
  public userId!:string;
  public userData!:UserSummaryDTO
  public comments!:CommentShortcutDTO[]
  public posts!:PostShortcutDTO[]

  public loadingPosts=true;
  public hasMorePosts=true;
  public postsPageSize = 5
  public postsPageNumber = 1;

  public loadingComments = true;
  public hasMoreComments=true;
  public commentsPageSize = 5
  public commentsPageNumber = 1;

  ngOnInit(): void {
      this.userId = this.route.snapshot.params['id'];
      this.userSummaryService.getUserSummary(this.userId).subscribe(data=>{
        this.userData=data;
      },
        error => this.toastr.error("Oops, couldn't retrieve user data"))

      this.commentsService.getShortcutsOfUser(this.userId).subscribe(data=>{
        this.comments=data;
        this.loadingComments=false
        console.log(this.comments)
      })
    this.postsService.getShortcutsOfUser(this.userId).subscribe(data=>{
      this.posts=data;
      this.loadingPosts=false
      console.log(this.posts)
    })
  }
  isAccountOwner(){
    return this.authService.getUser()?.userId==this.userId;
  }

  loadMoreComments() {
    this.commentsPageNumber++;
    this.commentsService.getShortcutsOfUser(this.userId,this.commentsPageNumber,this.commentsPageSize).subscribe(data=>{
      this.comments= this.comments.concat(data);
      this.hasMoreComments = data.length!=0;
      this.loadingComments=false
      console.log(this.comments)
    })
  }

  loadMorePosts() {
    this.postsPageNumber++;
    this.postsService.getShortcutsOfUser(this.userId,this.postsPageNumber,this.postsPageSize).subscribe(data=>{
      this.posts=this.posts.concat(data);
      this.hasMorePosts=data.length!=0;
      this.loadingPosts=false
      console.log(this.posts)
    })
  }
}
