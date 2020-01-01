import { HttpClient, HttpHeaders } from '@angular/common/http';
import {Component, Inject} from '@angular/core';
import { Router } from "@angular/router";
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {TokenHelpers} from "../../shared/helpers/token.helpers";

@Component({
  selector: 'auth-login-component',
  host: {
    class: "row col-12 m-0 p-0"
  },
  templateUrl: './login.component.html'
})
export class LoginComponent {
  invalidLogin: boolean = false;
  loginForm: FormGroup = new FormGroup({
    username: new FormControl("", [
      Validators.required, Validators.minLength(4), Validators.maxLength(20)
    ]),

    password: new FormControl("", [
      Validators.required, Validators.minLength(6), Validators.maxLength(20)
    ])
  });

  constructor(private router: Router,
              private http: HttpClient,
              @Inject("BASE_URL") private baseUrl: string) {}

  public login(){
    let credentials = JSON.stringify(this.loginForm.value);
    this.http.post<any>(this.baseUrl + "api/auth/login", credentials, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).subscribe(response => {
      TokenHelpers.TOKEN = response.token;
      this.invalidLogin = false;
      this.router.navigate(["/" + TokenHelpers.TOKEN_USERNAME]).then(() => {});
    }, () => {
      this.invalidLogin = true;
    });
  }
  public invalidForm(form: FormGroup){
    return form.invalid && form.touched;
  }
}
