import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {JwtHelperService} from "@auth0/angular-jwt";
import {switchMap} from "rxjs/operators";
import {TokenHelpers} from "../shared/helpers/token.helpers";

@Component({
  selector: 'app-blog-component',
  templateUrl: './blog.component.html',
  styleUrls: ['blog.component.css'],
  host: {
    class: "row"
  }
})

export class BlogComponent implements OnInit{
  private username: string;

  constructor(private jwtHelper: JwtHelperService,
              private router: Router,
              private activateRoute: ActivatedRoute,
              ) {}

  ngOnInit(): void {
    this.activateRoute.paramMap.pipe(
      switchMap(params => params.get('username'))
    ).subscribe(data=> this.username += data, () => this.router.navigateByUrl("" + TokenHelpers.TOKEN_USERNAME));
  }

  public isUserAuthenticated() {
    return TokenHelpers.IS_TOKEN_CORRECT;
  }

  public logOut = () => {
    TokenHelpers.removeToken();
    this.router.navigateByUrl("").then(() => {} );
  }
}
