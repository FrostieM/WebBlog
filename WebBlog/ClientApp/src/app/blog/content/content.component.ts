﻿import {Component, Inject, Input} from '@angular/core';
import { Router } from "@angular/router";
import {IUserPostsViewData} from "../../shared/interfaces/userPostsViewData.interface";
import {IPostViewData} from "../../shared/interfaces/postViewData.interface";
import {ServerService} from "../../shared/services/server.service";

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
  public mainPost: IPostViewData;

  private type: string;
  @Input()  public set Type(type: string){
    this.type = type == "home" ? null : type;
    this.tags = null;
    this.getPosts(1);
  };

  private tags: string[] = null;
  @Input() public set Tags(tags: string[]){
    this.tags = tags;
    this.getPosts(1)
  };

  constructor(private router: Router,
              @Inject("BASE_URL") private baseUrl: string,
              private serverService: ServerService) {
  }

  public getPosts(page: number = 1) {
    this.isForm = false;
    let obj = JSON.stringify({type: this.type, username: this.username, tags: this.tags, currentPage: page});

    this.serverService.getPosts(obj).toPromise().then(response => {
     this.userPosts = response;
     this.mainPost = this.userPosts.posts.shift();
    }, err => {
      console.log(err)
    });
  }
}
