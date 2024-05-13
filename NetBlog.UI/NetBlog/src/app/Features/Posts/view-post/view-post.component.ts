import {Component, OnInit} from '@angular/core';
import {PostDTO} from "../models/PostDTO";
import {PostsService} from "../services/PostService";
import {ActivatedRoute, Router, RouterLink} from "@angular/router";
import {DatePipe, NgForOf, NgIf} from "@angular/common";
import {AuthService} from "../../Auth/services/auth-service";
import {provideToastr, ToastrService} from "ngx-toastr";
import {ViewPostCommentsComponent} from "../Comments/view-post-comments/view-post-comments.component";

@Component({
  selector: 'app-view-post',
  standalone: true,
  imports: [
    DatePipe,
    NgForOf,
    NgIf,
    RouterLink,
    ViewPostCommentsComponent
  ],
  templateUrl: './view-post.component.html',
  styleUrl: './view-post.component.css'
})
export class ViewPostComponent implements OnInit{
  public post!:PostDTO;
  public postId:string;

  public constructor(private postService:PostsService,
                     private route: ActivatedRoute,
                     private authService:AuthService,
                     private toastr:ToastrService,
                     private router:Router
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

  delete() {
    this.postService.deletePost(this.postId).subscribe(result=>{
      this.toastr.success("Post deleted successfully")
      this.router.navigate(['/posts'])
    })
  }
}
