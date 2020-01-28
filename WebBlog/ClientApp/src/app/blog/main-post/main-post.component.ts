import { HttpClient } from '@angular/common/http';
import {Component, EventEmitter, Inject, Input, Output} from '@angular/core';
import { Router } from "@angular/router";
import {ILikeViewData} from "../../shared/interfaces/likeViewData.interface";
import {IPost} from "../../shared/interfaces/post.interface";


@Component({
  selector: 'blog-main-post-component',
  host: {
    class: ""
  },
  templateUrl: './main-post.component.html'
})
export class MainPostComponent {

  @Input() mainPost: ILikeViewData<IPost>;
  @Input() isCreator: boolean;

  @Output() public messageToUpdate = new EventEmitter();

  constructor(private router: Router,
              private http: HttpClient,
              @Inject("BASE_URL") private baseUrl: string) {

  }

  public updatePosts(){
    this.messageToUpdate.emit();
  }

  createFullSrc(fileUrl){
    return "https://localhost:5001/" + fileUrl;
  }
}
