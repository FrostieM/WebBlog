import { HttpClient } from '@angular/common/http';
import {Component, EventEmitter, Inject, Input, Output} from '@angular/core';
import { Router } from "@angular/router";
import {IPostViewData} from "../../shared/interfaces/postViewData.interface";

@Component({
  selector: 'blog-main-post-component',
  host: {
    class: ""
  },
  templateUrl: './main-post.component.html'
})
export class MainPostComponent {

  @Input() mainPost: IPostViewData;
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
