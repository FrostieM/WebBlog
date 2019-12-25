import {Component, Inject} from '@angular/core';
import {Router} from "@angular/router";
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-auth-component',
  templateUrl: './auth.component.html',
  styleUrls: ['auth.component.css']
})
export class AuthComponent {
  isLoginForm: boolean = true;

  constructor(private router: Router,
              private http: HttpClient,
              @Inject("BASE_URL") private baseUrl: string) {

    //if page was update when was signUp
    if (this.router.url.split("/").pop() == "signUp") this.isLoginForm = false;
  }
}
