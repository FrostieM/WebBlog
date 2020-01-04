import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import {Component, Inject, Input} from '@angular/core';
import { Router } from "@angular/router";
import {UserPostsViewDataInterface} from "../../shared/interfaces/userPostsViewData.interface";
import {PostViewDataInterface} from "../../shared/interfaces/postViewData.interface";
import {NgForm} from "@angular/forms";

@Component({
  selector: 'blog-content-component',
  host: {
    class: ""
  },
  templateUrl: './content.component.html'
})
export class ContentComponent{

  @Input() public username: string;

  public isViewRow: boolean = true;
  public isForm: boolean;
  public userPosts: UserPostsViewDataInterface;
  public mainPost: PostViewDataInterface;

  private type: string;
  @Input()  public set Type(type: string){
    this.type = type;
    this.getPosts(1);
  };

  constructor(private router: Router,
              private http: HttpClient,
              @Inject("BASE_URL") private baseUrl: string) {
  }

  getPosts(page: number = 1) {
    let postsParams = new HttpParams().set("currentPage", page.toString());
    this.http.get<UserPostsViewDataInterface>(this.baseUrl + "api/posts/" + this.type + "/" + this.username, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      }),
      params: postsParams
    }).toPromise().then(response => {
      console.log(response);
     this.userPosts = response;
     this.mainPost = this.userPosts.posts.shift();
      console.log(this.userPosts);
    }, err => {
      console.log(err)
    });
  }

  setPosts(ngForm: NgForm) {
    let post = JSON.stringify(ngForm.value);
    this.http.post<any>(this.baseUrl + "api/posts/savePost", post, {
      headers: new HttpHeaders({
        "Content-Type": "application/json",
      }),
      responseType: 'text' as 'json'
    }).subscribe(() => {
      this.getPosts(1);
      this.isForm=false;
    }, err => {
      console.log(err)
    });
  }
}
