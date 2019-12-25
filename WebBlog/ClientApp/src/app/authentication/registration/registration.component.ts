import { HttpClient, HttpHeaders } from '@angular/common/http';
import {Component, Inject} from '@angular/core';
import { Router } from "@angular/router";
import { NgForm } from '@angular/forms';

@Component({
  selector: 'auth-registration-component',
  host: {
    class: "row col-12 m-0 p-0"
  },
  templateUrl: './registration.component.html'
})
export class RegistrationComponent {
  invalidAuth: boolean = false;

  constructor(private router: Router,
              private http: HttpClient,
              @Inject("BASE_URL") private baseUrl: string) { }

  public registration(form: NgForm){
    let credentials = JSON.stringify(form.value);
    this.http.post<any>(this.baseUrl + "api/auth/login", credentials, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).subscribe(response => {
      localStorage.setItem("Token", response.token);
      this.invalidAuth = false;
      this.router.navigate(["/"]).then(() => {});
    }, () => {
      this.invalidAuth = true;
    });
  }

}
