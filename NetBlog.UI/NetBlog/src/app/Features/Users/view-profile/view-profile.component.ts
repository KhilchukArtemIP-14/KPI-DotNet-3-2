import {Component, OnInit} from '@angular/core';
import {UserSummaryService} from "../Services/user-summary-service";
import {AuthService} from "../../Auth/services/auth-service";
import {ActivatedRoute, Router, RouterLink} from "@angular/router";
import {UserSummaryDTO} from "../Models/user-summary-dto";
import {ToastrService} from "ngx-toastr";
import {NgForOf, NgIf} from "@angular/common";

@Component({
  selector: 'app-view-profile',
  standalone: true,
  imports: [
    NgForOf,
    NgIf,
    RouterLink
  ],
  templateUrl: './view-profile.component.html',
  styleUrl: './view-profile.component.css'
})
export class ViewProfileComponent implements OnInit{
  public constructor(private userSummaryService:UserSummaryService,
                     private authService:AuthService,
                     private route:ActivatedRoute,
                     private router:Router,
                     private toastr:ToastrService) {
  }
  public userId!:string;
  public userData!:UserSummaryDTO
  ngOnInit(): void {
      this.userId = this.route.snapshot.params['id'];
      this.userSummaryService.getUserSummary(this.userId).subscribe(data=>{
        this.userData=data;
      },
        error => this.toastr.error("Oops, couldn't retrieve user data"))
  }
  isAccountOwner(){
    return this.authService.getUser()?.userId==this.userId;
  }
}
