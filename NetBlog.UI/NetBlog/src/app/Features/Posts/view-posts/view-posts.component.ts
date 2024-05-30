import {Component, HostListener} from '@angular/core';
import {PostsService} from "../services/post-service";
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
  private pageSize = 5;
  private pageNumber = 1;
  public isLoading = true;
  public hasMore = true;
  private searchQuery: string | null = null;

  ngOnInit(): void {
    this.postsService
      .getPostSummaries()
      .subscribe(data=> {
        this.posts=data;
        this.isLoading=false;
      })
  }
  public isAuthor(){
    return this.authService.getUser()?.roles.includes("Author")
  }

  @HostListener('window:scroll', [])
  onScroll(): void {
    const position = window.scrollY + window.innerHeight;
    const height = document.documentElement.scrollHeight;
    if (!this.isLoading && this.hasMore && position >= height) {
      this.pageNumber++;
      this.isLoading=true;
      this.postsService.getPostSummaries(this.pageNumber,this.pageSize,this.searchQuery,false).subscribe(data=>{
        this.hasMore = data.length!=0;
        this.posts = this.posts.concat(data);
        this.isLoading=false;
      })
    }
  }

  onSearch(value: string) {
    this.pageNumber=1;
    this.searchQuery = value;
    this.isLoading=true;
    this.hasMore = true;
    this.postsService.getPostSummaries(this.pageNumber,this.pageSize,this.searchQuery,false).subscribe(data=>{
      this.posts=data;
      console.log(data)
      this.isLoading=false;
      this.hasMore=data.length!=0;
    })
  }
}
