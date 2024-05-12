import { Routes } from '@angular/router';
import {HomepageComponent} from "./Features/homepage/homepage.component";
import {ViewPostsComponent} from "./Features/Posts/view-posts/view-posts.component";
import {ViewPostComponent} from "./Features/Posts/view-post/view-post.component";
import {CreatePostComponent} from "./Features/Posts/create-post/create-post.component";
import {RegisterComponent} from "./Features/Auth/register/register.component";
import {LoginComponent} from "./Features/Auth/login/login.component";
import {EditPostComponent} from "./Features/Posts/edit-post/edit-post.component";

export const routes: Routes = [
  {
    path:"",
    component:HomepageComponent
  },
  {
    path:"posts",
    component:ViewPostsComponent
  },
  {
    path:"posts/create",
    component:CreatePostComponent
  },
  {
    path:"posts/:id",
    component:ViewPostComponent
  },
  {
    path:"posts/:id/edit",
    component:EditPostComponent
  },
  {
    path:"register",
    component:RegisterComponent
  },
  {
    path:"login",
    component:LoginComponent
  },
];
