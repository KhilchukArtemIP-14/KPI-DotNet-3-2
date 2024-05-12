import { Routes } from '@angular/router';
import {HomepageComponent} from "./Features/homepage/homepage.component";
import {ViewPostsComponent} from "./Features/Posts/view-posts/view-posts.component";

export const routes: Routes = [
  {
    path:"",
    component:HomepageComponent
  },
  {
    path:"posts",
    component:ViewPostsComponent
  }
];
