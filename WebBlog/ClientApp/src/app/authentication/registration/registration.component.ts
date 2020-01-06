import { HttpClient, HttpHeaders } from '@angular/common/http';
import {Component, Inject} from '@angular/core';
import { Router } from "@angular/router";
import { FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {TokenHelpers} from "../../shared/services/helpers/token-helper.service";

@Component({
  selector: 'auth-registration-component',
  host: {
    class: "row col-12 m-0 p-0"
  },
  templateUrl: './registration.component.html'
})
export class RegistrationComponent {
  invalidSignUp: boolean = false;
  signUpForm: FormGroup;

  constructor(private router: Router,
              private http: HttpClient,
              private fb: FormBuilder,
              @Inject("BASE_URL") private baseUrl: string) {

    this.signUpForm = fb.group({
      username: new FormControl("", [
        Validators.required, Validators.minLength(4), Validators.maxLength(20)
      ]),

      password: new FormControl("", [
        Validators.required, Validators.minLength(6), Validators.maxLength(20)
      ]),

      rePassword: new FormControl("", ),

      firstname: new FormControl("", [
        Validators.required,
      ]),

      lastname: new FormControl("", [
        Validators.required,
      ]),

      email: new FormControl( "", [
        Validators.required, Validators.email
      ]),
    }, {
      validators: this.checkPasswords
    });
  }

  public registration(){
    let credentials = JSON.stringify(this.signUpForm.value);
    this.http.post<any>(this.baseUrl + "api/auth/signUp", credentials, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).subscribe(response => {
      TokenHelpers.TOKEN = response.token;
      this.invalidSignUp = false;
      this.router.navigate(["/" + TokenHelpers.TOKEN_USERNAME]).then(() => {});
    }, () => {
      this.invalidSignUp = true;
    });
  }

  public invalidForm(form: FormGroup){
    return form.invalid && form.touched;
  }

  checkPasswords(form: FormGroup) { // here we have the 'passwords' group
    let pass = form.get('password').value;
    let confirmPass = form.get('rePassword').value;

    return pass === confirmPass ? null : { notSame: true }
  }

}
