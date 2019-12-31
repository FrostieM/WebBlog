import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import {CommonModule} from "@angular/common";
import {LoginComponent} from "./login/login.component";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {AuthComponent} from "./auth.component";
import {RegistrationComponent} from "./registration/registration.component";
import {RoleGuard} from "../shared/services/guards/role-guard.service";


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
            {
                path: 'auth', component: AuthComponent, canActivate: [RoleGuard], children:
                    [
                        {path: '', component: LoginComponent},
                        {path: 'signUp', component: RegistrationComponent}
                    ]
            },
        ]),
        ReactiveFormsModule
    ],
  providers: [RoleGuard],
  exports: []
})
export class AuthModule { }

