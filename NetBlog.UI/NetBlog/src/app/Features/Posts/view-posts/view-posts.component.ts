import { Component } from '@angular/core';
import {PostsService} from "../services/PostService";
import {PostSummaryDTO} from "../models/PostSummaryDTO";
import {DatePipe, NgForOf, NgIf} from "@angular/common";
import {AuthService} from "../../Auth/services/auth-service";
import {RouterLink} from "@angular/router";
import {PostDTO} from "../models/PostDTO";
import {state} from "@angular/animations";

@Component({
  selector: 'app-view-posts',
  standalone: true,
  imports: [
    DatePipe,
    NgForOf,
    RouterLink,
    NgIf
  ],
  templateUrl: './view-posts.component.html',
  styleUrl: './view-posts.component.css'
})
export class ViewPostsComponent {
  public constructor(private postsService:PostsService,
                     private authService:AuthService) {
  }
  public posts:PostSummaryDTO[]=[]
  ngOnInit(): void {
    this.postsService
      .getPostSummaries()
      .subscribe(data=> this.posts=data)
  }
  public isAuthor(){
    return this.authService.getUser()?.roles.includes("Author")
  }
  public authoredPost(post:PostSummaryDTO){
    return this.isAuthor()&&post.createdBy.userId==this.authService.getUser()?.userId
  }

  protected readonly state = state;
}
