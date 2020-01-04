import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import {Component, EventEmitter, Inject, Input, Output} from '@angular/core';
import { Router } from "@angular/router";
import {PostViewDataInterface} from "../../shared/interfaces/postViewData.interface";

@Component({
  selector: 'blog-post-info-component',
  host: {
    class: "col-auto ml-auto p-0 m-0"
  },
  templateUrl: './post-info.component.html'
})
export class PostInfoComponent {

  @Input() public isCreator: boolean;
  @Input() public postViewData: PostViewDataInterface;
  @Output() public messageToUpdate = new EventEmitter();


  constructor(private router: Router,
              private http: HttpClient,
              @Inject("BASE_URL") private baseUrl: string) {

  }

  public deletePost(id: number){
    let params = new HttpParams().set("id", id.toString());
    this.http.get<any>(this.baseUrl + "api/posts/deletePost", {
      headers: new HttpHeaders({
        "Content-Type": "application/json",
      }),
      params: params,
      responseType: 'text' as 'json'
    }).subscribe(() => {
      this.messageToUpdate.emit();
    }, err => {
      console.log(err)
    });
  }
}
