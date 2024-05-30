import {Component, HostListener, OnInit} from '@angular/core';
import {PostsService} from "../services/post-service";
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
  private pageSize = 5;
  private pageNumber = 1;
  public isLoading = true;
  public hasMore = true;
  ngOnInit(): void {
        this.postsService
          .getPostSummaries()
          .subscribe(data=>{
            this.posts=data;
          this.isLoading=false
          } )
    }
  @HostListener('window:scroll', [])
  onScroll(): void {
    const position = window.scrollY + window.innerHeight;
    const height = document.documentElement.scrollHeight;
    if (!this.isLoading && this.hasMore && position >= height) {
      this.pageNumber++;
      this.isLoading=true;
      this.postsService.getPostSummaries(this.pageNumber,this.pageSize,null,false).subscribe(data=>{
        this.hasMore = data.length!=0;
        this.posts = this.posts.concat(data);
        this.isLoading=false;
      })
    }
  }
}
