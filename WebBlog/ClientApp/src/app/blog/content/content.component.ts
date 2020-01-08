import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Component, Inject, Input} from '@angular/core';
import { Router } from "@angular/router";
import {UserPostsViewDataInterface} from "../../shared/interfaces/userPostsViewData.interface";
import {PostViewDataInterface} from "../../shared/interfaces/postViewData.interface";

@Component({
  selector: 'blog-content-component',
  host: {
    class: ""
  },
  templateUrl: './content.component.html'
})
export class ContentComponent{

  @Input() public username: string;
  @Input() public isCreator: boolean;

  public isViewRow: boolean = false;
  public isForm: boolean;
  public userPosts: UserPostsViewDataInterface;
  public mainPost: PostViewDataInterface;

  public pages: number[];

  private pageRange: number = 2;
  private type: string;
  @Input()  public set Type(type: string){
    this.type = type;
    this.getPosts(1);
  };

  private tags: string[] = null;
  @Input() public set Tags(tags: string[]){
    this.tags = tags;
    this.getPosts(1)
  };

  constructor(private router: Router,
              private http: HttpClient,
              @Inject("BASE_URL") private baseUrl: string) {
  }

  getPosts(page: number = 1) {
    this.isForm = false;

    let obj = JSON.stringify({type: this.type, username: this.username, tags: this.tags, currentPage: page});

    this.http.post<UserPostsViewDataInterface>(this.baseUrl + "api/posts/", obj , {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })

    }).toPromise().then(response => {
     this.userPosts = response;
     this.mainPost = this.userPosts.posts.shift();
     this.pages = this.range(this.userPosts.pagingInfo.currentPage - this.pageRange, this.userPosts.pagingInfo.currentPage + this.pageRange)
       .filter(p => p > 0 && p <= this.userPosts.pagingInfo.totalPages);
    }, err => {
      console.log(err)
    });
    console.log(this.pages);
  }

  public isCurrentPage(currentPage: number, page: number){
    return currentPage == page;
  }

  public range(start: number, end: number, length: number = end - start + 1){
    return Array.from({length}, (_, i) => start + i);
  }

  public dotsPageLeft(itemPage: number, currentPage: number){
    let dotsLength = Math.abs(itemPage - currentPage);
    return ".".repeat(dotsLength < 5 ? dotsLength : 5);
  }

}
