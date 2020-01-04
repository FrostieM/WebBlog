import { HttpClient } from '@angular/common/http';
import {Component, EventEmitter, Inject, Input, Output} from '@angular/core';
import { Router } from "@angular/router";
import {PostViewDataInterface} from "../../shared/interfaces/postViewData.interface";

@Component({
  selector: 'blog-post-card-component',
  host: {
    class: ""
  },
  templateUrl: './post-card.component.html'
})
export class PostCardComponent {

  @Input() public postViewData: PostViewDataInterface;
  @Input() public isCreator: boolean;

  @Output() public messageToUpdate = new EventEmitter();

  public get SubDescription(){
    return this.postViewData.post.description.substring(0, 200);
  }
  constructor(private router: Router,
              private http: HttpClient,
              @Inject("BASE_URL") private baseUrl: string) {

  }

  public updatePosts(){
    this.messageToUpdate.emit();
  }
}
