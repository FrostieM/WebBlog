import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable()
export class AuthGuard implements CanActivate {

  constructor(private jwtHelper: JwtHelperService, private router: Router) {
  }
  canActivate() {
    let token = localStorage.getItem("Token");

    if (token && !this.jwtHelper.isTokenExpired(token)){
      console.log(this.jwtHelper.decodeToken(token));
      return true;
    }
    this.router.navigate(["auth"]).then(() => {});
    return false;
  }

}
