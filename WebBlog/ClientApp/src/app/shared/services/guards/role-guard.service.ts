import {Injectable} from "@angular/core";

import { CanActivate, Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable()
export class RoleGuard implements CanActivate {
  constructor(private jwtHelper: JwtHelperService, private router: Router) {}
  canActivate() {
    let user = JSON.parse(localStorage.getItem("user"));

    if (!user){
      return true;
    }
    this.router.navigate([""]).then(() => {});
    return false;
  }
}
