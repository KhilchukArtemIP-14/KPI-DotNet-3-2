import {Component, OnInit} from '@angular/core';
import {PostsService} from "../services/PostService";
import {PostSummaryDTO} from "../models/PostSummaryDTO";
import {DatePipe, NgForOf} from "@angular/common";

@Component({
  selector: 'app-view-recent-posts',
  standalone: true,
  imports: [
    NgForOf,
    DatePipe
  ],
  templateUrl: './view-recent-posts.component.html',
  styleUrl: './view-recent-posts.component.css'
})
export class ViewRecentPostsComponent implements OnInit{
  public constructor(private postsService:PostsService) {
  }
  public posts:PostSummaryDTO[]=[]
  ngOnInit(): void {
        this.postsService
          .getPostSummaries()
          .subscribe(data=> this.posts=data)
    }
}
