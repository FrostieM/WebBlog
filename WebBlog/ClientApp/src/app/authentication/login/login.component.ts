import {Component, Inject} from '@angular/core';
import { Router } from "@angular/router";
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {TokenService} from "../../shared/services/token.service";
import {AuthService} from "../../shared/services/auth.service";

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
              @Inject("BASE_URL") private baseUrl: string,
              private tokenService: TokenService,
              private authService: AuthService) {}

  public login(){
    let credentials = JSON.stringify(this.loginForm.value);

    this.authService.login(credentials).subscribe(response => {
      this.tokenService.addToken(response.token);
      this.invalidLogin = false;
      this.router.navigate(["/" + this.tokenService.Username]).then(() => {});
    }, () => {
      this.invalidLogin = true;
    });
  }
  public invalidForm(form: FormGroup){
    return form.invalid && form.touched;
  }
}
