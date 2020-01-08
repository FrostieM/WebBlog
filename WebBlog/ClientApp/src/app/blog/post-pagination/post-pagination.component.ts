import {HttpClient} from '@angular/common/http';
import {Component, EventEmitter, Inject, Input, Output} from '@angular/core';
import { Router } from "@angular/router";
import {PagingInfoInterface} from "../../shared/interfaces/pagingInfo.interface";

@Component({
  selector: 'blog-post-pagination-component',
  host: {
    class: "row"
  },
  templateUrl: './post-pagination.component.html'
})
export class PostPaginationComponent{

  @Output() public messageToUpdate = new EventEmitter();

  public pages: number[];
  private pageRange: number = 2;

  private pagingInfo: PagingInfoInterface;
  @Input() set PagingInfo(pagingInfo: PagingInfoInterface){
    this.pagingInfo = pagingInfo;
    this.pages = this.range(this.pagingInfo.currentPage - this.pageRange, this.pagingInfo.currentPage + this.pageRange)
      .filter(p => p > 0 && p <= this.pagingInfo.totalPages);
  }


  constructor(private router: Router,
              private http: HttpClient,
              @Inject("BASE_URL") private baseUrl: string) {
  }


  public isCurrentPage(currentPage: number, page: number){
    return currentPage == page;
  }

  public range(start: number, end: number, length: number = end - start + 1){
    return Array.from({length}, (_, i) => start + i);
  }

  public dotsPageLeft(itemPage: number, currentPage: number){
    let dotsLength = Math.abs(itemPage - currentPage);
    return ".".repeat(dotsLength < 5 ? dotsLength : 5);
  }

  public updatePosts(page: number){
    this.messageToUpdate.emit(page);
  }

}
