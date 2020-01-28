import { HttpClient } from '@angular/common/http';
import {Component, EventEmitter, Inject, Input, OnInit, Output} from '@angular/core';
import { Router } from "@angular/router";
import {ILikeViewData} from "../../shared/interfaces/likeViewData.interface";
import {IPost} from "../../shared/interfaces/post.interface";


@Component({
  selector: 'blog-post-card-component',
  styleUrls: ['post-card.component.css'],
  host: {
    class: ""
  },
  templateUrl: './post-card.component.html'
})
export class PostCardComponent implements OnInit{

  @Input() public postViewData: ILikeViewData<IPost>;
  @Input() public mainPost: ILikeViewData<IPost>;

  @Input() public isCreator: boolean = false;
  @Input() public isViewRow: boolean = false;

  @Output() public messageToUpdate = new EventEmitter();

  @Output() public postViewDataChange = new EventEmitter();
  @Output() public mainPostChange = new EventEmitter();

  private video_bg: string = "1";

  public get SubDescription(){
    return this.postViewData.item.description.substring(0, 270);
  }

  constructor(private router: Router,
              private http: HttpClient,
              @Inject("BASE_URL") private baseUrl: string) {}

  ngOnInit(): void {
    this.video_bg = this.randomItem(['1', '2', '3', '4']); //random video background in assets/img/video_bg
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

  public changePost(){
    [this.postViewData, this.mainPost] = [this.mainPost, this.postViewData];
    this.postViewDataChange.emit(this.postViewData);
    this.mainPostChange.emit(this.mainPost);
  }
}
