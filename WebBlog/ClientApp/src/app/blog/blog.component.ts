import {Component, OnInit, ViewEncapsulation} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {JwtHelperService} from "@auth0/angular-jwt";
import {TokenHelpers} from "../shared/services/helpers/token-helper.service";


@Component({
  selector: 'app-blog-component',
  templateUrl: './blog.component.html',
  styleUrls: ['blog.component.css'],
  host: {
    class: "container-fluid d-flex"
  },
  encapsulation: ViewEncapsulation.None
})

export class BlogComponent implements OnInit{
  private username: string;

  constructor(private jwtHelper: JwtHelperService,
              private router: Router,
              private activateRoute: ActivatedRoute,
              ) {}

  ngOnInit(): void {
    this.username = this.activateRoute.snapshot.paramMap.get('username');
    if (!this.username) this.router.navigateByUrl("" + TokenHelpers.TOKEN_USERNAME).then(() => {}); //if url was blank use username in token and go to it
  }

  public isUserAuthenticated() {
    return TokenHelpers.IS_TOKEN_CORRECT;
  }

  public logOut = () => {
    TokenHelpers.removeToken();
    this.router.navigateByUrl("").then(() => {} );
  }
}
