﻿import {Injectable} from "@angular/core";

import { CanActivate, Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable()
export class AuthGuard implements CanActivate {
  constructor(private jwtHelper: JwtHelperService, private router: Router) {}
  canActivate() {
    let user = JSON.parse(localStorage.getItem("user"));

    if (user && user.token && !this.jwtHelper.isTokenExpired(user.token)){
      return true;
    }
    this.router.navigate(["auth"]).then(() => {});
    return false;
  }
}
