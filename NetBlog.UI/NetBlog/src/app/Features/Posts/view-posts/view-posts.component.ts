import { Component } from '@angular/core';
import {PostsService} from "../services/PostService";
import {PostSummaryDTO} from "../models/PostSummaryDTO";
import {DatePipe, NgForOf} from "@angular/common";

@Component({
  selector: 'app-view-posts',
  standalone: true,
  imports: [
    DatePipe,
    NgForOf
  ],
  templateUrl: './view-posts.component.html',
  styleUrl: './view-posts.component.css'
})
export class ViewPostsComponent {
  public constructor(private postsService:PostsService) {
  }
  public posts:PostSummaryDTO[]=[]
  ngOnInit(): void {
    this.postsService
      .getPostSummaries()
      .subscribe(data=> this.posts=data)
  }
}
