import { HttpClient } from '@angular/common/http';
import {Component, EventEmitter, Inject, Input, Output} from '@angular/core';
import { Router } from "@angular/router";

@Component({
  selector: 'blog-menu-button-component',
  host: {
    class: "w-100 mt-3"
  },
  templateUrl: './menu-button.component.html'
})
export class MenuButtonComponent {

  @Input() public buttonType: string;
  @Input() public currentType: string;

  @Input() public d: string;
  @Input() public transform: string;
  @Output() messageToEmit = new EventEmitter<string>();

  constructor(private router: Router,
              private http: HttpClient,
              @Inject("BASE_URL") private baseUrl: string) {
  }

  public isButtonActive(){
    return this.buttonType == this.currentType;
  }

  public sendMessageToParent(message: string) {
    this.messageToEmit.emit(message);
  }
}
