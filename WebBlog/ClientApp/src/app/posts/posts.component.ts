import {Component, Inject, OnInit} from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-posts',
  templateUrl: './posts.component.html',
  styleUrls: []
})
export class PostsComponent  {
  constructor(private http: HttpClient, @Inject("BASE_URL") private baseUrl: string) { }

  getPosts() {
    this.http.get(this.baseUrl + "api/posts", {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      }),
      params: {
        username: "admin"
      }
    }).subscribe(response => {
      console.log(response);
    }, err => {
      console.log(err)
    });
  }

  setPosts() {
    let object = JSON.stringify({
      type: "test type",
      title: "test title",
      description: "test description",
      fileUrl: "testUrl"
    });
    this.http.post(this.baseUrl + "api/posts/savePost", object, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).subscribe(response => {
      console.log(response);
    }, err => {
      console.log(err)
    });
  }



}
