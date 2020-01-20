import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';

import {AuthModule} from "./authentication/auth.module";

import {BlogModule} from "./blog/blog.module";

import {JwtHelperService, JwtModule} from "@auth0/angular-jwt";
import {TokenService} from "./shared/services/token.service";

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    AuthModule,
    BlogModule,
    RouterModule.forRoot([]),
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        whitelistedDomains: ["localhost:5001"],
        blacklistedRoutes: []
      }
    })
  ],
  providers: [TokenService, JwtHelperService],
  bootstrap: [AppComponent]
})
export class AppModule { }

export function tokenGetter() {
  return new TokenService().Token;
}
