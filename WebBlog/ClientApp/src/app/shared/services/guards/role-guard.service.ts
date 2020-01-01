import {Injectable} from "@angular/core";

import { CanActivate, Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import {TokenHelpers} from "../helpers/token-helper.service";

@Injectable()
export class RoleGuard implements CanActivate {
  constructor(private jwtHelper: JwtHelperService, private router: Router) {}
  canActivate() {

    if (!TokenHelpers.IS_TOKEN_CORRECT){
      return true;
    }

    this.router.navigate([""]).then(() => {});
    return false;
  }
}
