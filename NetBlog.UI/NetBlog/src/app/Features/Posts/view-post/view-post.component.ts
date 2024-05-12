import {Component, OnInit} from '@angular/core';
import {PostDTO} from "../models/PostDTO";
import {PostsService} from "../services/PostService";
import {ActivatedRoute, RouterLink} from "@angular/router";
import {DatePipe, NgForOf, NgIf} from "@angular/common";
import {AuthService} from "../../Auth/services/auth-service";

@Component({
  selector: 'app-view-post',
  standalone: true,
  imports: [
    DatePipe,
    NgForOf,
    NgIf,
    RouterLink
  ],
  templateUrl: './view-post.component.html',
  styleUrl: './view-post.component.css'
})
export class ViewPostComponent implements OnInit{
  public post!:PostDTO;
  public postId:string;

  public constructor(private postService:PostsService,
                     private route: ActivatedRoute,
                     private authService:AuthService
  ) {
    this.postId = this.route.snapshot.params['id'];
  }
  public canEdit(){
    var user = this.authService.getUser()
    return user?.roles.includes("Author")&&user?.userId==this.post.createdBy.userId;
  }
  ngOnInit(): void {
      this.postService
        .getPostById(this.postId)
        .subscribe(data=> {this.post=data; console.log(data)})
  }
}
