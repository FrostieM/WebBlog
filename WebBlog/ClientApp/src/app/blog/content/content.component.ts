import {Component, Inject, Input} from '@angular/core';
import { Router } from "@angular/router";
import {IUserPostsViewData} from "../../shared/interfaces/userPostsViewData.interface";

import {ServerService} from "../../shared/services/server.service";

import {IPost} from "../../shared/interfaces/post.interface";
import {IInfoItem} from "../../shared/interfaces/info-item.interface";
import {UserPostsViewData} from "../../shared/classes/userPostsViewData.class";

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
  public userPosts: IUserPostsViewData;
  public mainPost: IInfoItem<IPost>;

  private tags: string[] = null;
  @Input() public set Tags(tags: string[]){
    this.tags = tags;
    this.getPosts(1)
  };

  private type: string;
  @Input()  public set Type(type: string){
    this.type = type == "home" ? null : type;
    this.tags = null;
    this.getPosts(1);
  };

  constructor(private router: Router,
              @Inject("BASE_URL") private baseUrl: string,
              private serverService: ServerService) {
  }

  public getPosts(page: number = 1) {
    this.isForm = false;

    this.serverService.getPosts(this.type, this.username,this.tags, page)
      .toPromise().then(response => {
        this.userPosts = new UserPostsViewData(response);
        this.mainPost = this.userPosts.posts.shift();
        }, err => {
        console.log(err)
      });
  }
}
