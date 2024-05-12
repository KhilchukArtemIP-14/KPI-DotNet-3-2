import {Component, OnInit} from '@angular/core';
import {PostDTO} from "../models/PostDTO";
import {PostsService} from "../services/PostService";
import {ActivatedRoute} from "@angular/router";
import {DatePipe, NgForOf, NgIf} from "@angular/common";

@Component({
  selector: 'app-view-post',
  standalone: true,
  imports: [
    DatePipe,
    NgForOf,
    NgIf
  ],
  templateUrl: './view-post.component.html',
  styleUrl: './view-post.component.css'
})
export class ViewPostComponent implements OnInit{
  public post!:PostDTO;
  public postId:string;
  public constructor(private postService:PostsService,
                     private route: ActivatedRoute,
  ) {
    this.postId = this.route.snapshot.params['id'];
  }

  ngOnInit(): void {
      this.postService
        .getPostById(this.postId)
        .subscribe(data=> {this.post=data; console.log(data)})
  }
}
