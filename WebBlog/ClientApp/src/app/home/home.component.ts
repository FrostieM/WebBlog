import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import {switchMap} from "rxjs/operators";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit{
  private username: string;

  constructor(private jwtHelper: JwtHelperService, private router: Router, private activateRoute: ActivatedRoute) {}

  ngOnInit(): void {
    this.activateRoute.paramMap.pipe(
      switchMap(params => params.get('username'))
    ).subscribe(data=> this.username = data, () => this.router.navigateByUrl(JSON.parse(localStorage.getItem("user")).username));
  }

  isUserAuthenticated() {
    let user = JSON.parse(localStorage.getItem("user"));
    return user && user.token && !this.jwtHelper.isTokenExpired(user.token);
  }

  public logOut = () => {
    localStorage.removeItem("user");
  }
}
