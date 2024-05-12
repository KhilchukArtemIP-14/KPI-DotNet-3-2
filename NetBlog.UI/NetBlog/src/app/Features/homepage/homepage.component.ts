import { Component } from '@angular/core';
import {ViewRecentPostsComponent} from "../Posts/view-recent-posts/view-recent-posts.component";

@Component({
  selector: 'app-homepage',
  standalone: true,
  imports: [
    ViewRecentPostsComponent
  ],
  templateUrl: './homepage.component.html',
  styleUrl: './homepage.component.css'
})
export class HomepageComponent {

}
