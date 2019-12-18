import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  constructor(private jwtHelper: JwtHelperService, private router: Router) {
  }

  isUserAuthenticated() {
    let token: string = localStorage.getItem("Token");
    return token && !this.jwtHelper.isTokenExpired(token);
  }

  public logOut = () => {
    localStorage.removeItem("Token");
  }
}
