import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import {CommonModule} from "@angular/common";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";

import {AuthComponent} from "./auth.component";
import {LoginComponent} from "./login/login.component";
import {RegistrationComponent} from "./registration/registration.component";

import {AuthService} from "../shared/services/auth.service";
import {RoleGuard} from "../shared/guards/role.guard";


@NgModule({
  declarations: [
    AuthComponent,
    LoginComponent,
    RegistrationComponent
  ],
    imports: [
      CommonModule,
      FormsModule,
      ReactiveFormsModule,
      RouterModule.forRoot([
        {
          path: 'auth', component: AuthComponent, canActivate: [RoleGuard], children: [
            {path: '', component: LoginComponent},
            {path: 'signUp', component: RegistrationComponent}]
        },
      ])
    ],
  providers: [RoleGuard, AuthService],
  exports: []
})
export class AuthModule { }

