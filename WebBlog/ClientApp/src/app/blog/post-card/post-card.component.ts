import { HttpClient } from '@angular/common/http';
import {Component, EventEmitter, Inject, Input, OnInit, Output} from '@angular/core';
import { Router } from "@angular/router";
import {PostViewDataInterface} from "../../shared/interfaces/postViewData.interface";

@Component({
  selector: 'blog-post-card-component',
  styleUrls: ['post-card.component.css'],
  host: {
    class: ""
  },
  templateUrl: './post-card.component.html'
})
export class PostCardComponent implements OnInit{

  @Input() public postViewData: PostViewDataInterface;
  @Input() public isCreator: boolean = false;
  @Input() public isViewRow: boolean = false;

  @Output() public messageToUpdate = new EventEmitter();

  private video_bg: string = "1";
  public get SubDescription(){
    return this.postViewData.post.description.substring(0, 100);
  }
  constructor(private router: Router,
              private http: HttpClient,
              @Inject("BASE_URL") private baseUrl: string) {}

  ngOnInit(): void {
    this.video_bg = this.randomItem(['1', '2', '3', '4']);
  }

  public updatePosts(){
    this.messageToUpdate.emit();
  }

  public createFullSrc(fileUrl){
    return "https://localhost:5001/" + fileUrl;
  }

  public randomItem(items: any[]){
    return items[Math.floor(Math.random()*items.length)];
  }



}
