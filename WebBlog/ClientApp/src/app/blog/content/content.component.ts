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

  private type: string;
  @Input()  public set Type(type: string){
    this.type = type;
    this.getPosts(1);
  };

  @Input() public set Tags(tags: string[]){
    this.getPosts(1, tags)
  };

  constructor(private router: Router,
              private http: HttpClient,
              @Inject("BASE_URL") private baseUrl: string) {
  }

  getPosts(page: number = 1, sendTags: string[] = null) {
    this.isForm = false;

    if (sendTags != null){
      let j = JSON.stringify(sendTags);
      console.log(j);
    }

    let obj = JSON.stringify({type: this.type, username: this.username, tags: sendTags, currentPage: page});

    this.http.post<UserPostsViewDataInterface>(this.baseUrl + "api/posts/", obj , {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })

    }).toPromise().then(response => {
     this.userPosts = response;
     this.mainPost = this.userPosts.posts.shift();
    }, err => {
      console.log(err)
    });
  }

}
