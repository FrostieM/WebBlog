import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import {CommonModule} from "@angular/common";
import {LoginComponent} from "./login/login.component";
import {FormsModule} from "@angular/forms";
import {AuthComponent} from "./auth.component";
import {RegistrationComponent} from "./registration/registration.component";


@NgModule({
  declarations: [
    AuthComponent,
    LoginComponent,
    RegistrationComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    RouterModule.forRoot([
      { path: 'auth', component: AuthComponent, children:
          [
            { path: '', component: LoginComponent },
            { path: 'signUp', component: RegistrationComponent }
          ]},
    ])
  ],
  exports: []
})
export class AuthModule { }

