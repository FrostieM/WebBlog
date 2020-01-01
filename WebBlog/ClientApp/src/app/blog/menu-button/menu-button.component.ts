import { HttpClient } from '@angular/common/http';
import {Component, Inject, Input} from '@angular/core';
import { Router } from "@angular/router";

@Component({
  selector: 'blog-menu-button-component',
  host: {
    class: "w-100 mt-3"
  },
  templateUrl: './menu-button.component.html'
})
export class MenuButtonComponent {

  @Input() public name: string;
  @Input() public d: string;
  @Input() public transform: string;

  constructor(private router: Router,
              private http: HttpClient,
              @Inject("BASE_URL") private baseUrl: string) {}
}
