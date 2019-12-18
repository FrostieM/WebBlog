import { HttpClient, HttpHeaders } from '@angular/common/http';
import {Component, Inject} from '@angular/core';
import { Router } from "@angular/router";
import { NgForm } from '@angular/forms';

@Component({
  selector: 'login',
  templateUrl: './login.component.html'
})
export class LoginComponent {
  invalidLogin: boolean;

  constructor(private router: Router,
              private http: HttpClient,
              @Inject("BASE_URL") private baseUrl: string) { }

  public login = (form: NgForm) => {
    let credentials = JSON.stringify(form.value);
    this.http.post<any>(this.baseUrl + "api/auth/login", credentials, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).subscribe(response => {
      localStorage.setItem("Token", response.token);
      this.invalidLogin = false;
      this.router.navigate(["/"]).then(() => {});
    }, err => {
      this.invalidLogin = true;
    });
  }
}
