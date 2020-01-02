import {Component, Inject, OnInit} from '@angular/core';
import {Router} from "@angular/router";
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-auth-component',
  templateUrl: './auth.component.html',
  styleUrls: ['auth.component.css'],
  host: {
    class: 'container'
  }
})
export class AuthComponent implements OnInit{

  isLoginForm: boolean = true;

  constructor(private router: Router,
              private http: HttpClient,
              @Inject("BASE_URL") private baseUrl: string) {}

  ngOnInit(): void {
    //if page was update when was signUp
    this.isLoginForm = this.router.url.split("/").pop() != "signUp";
  }
}
