import {Component, Inject, OnInit, ViewEncapsulation} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {JwtHelperService} from "@auth0/angular-jwt";
import {TagViewData} from "../shared/classes/tagViewData.class";
import {TokenService} from "../shared/services/token.service";
import {ServerService} from "../shared/services/server.service";
import {HttpParams} from "@angular/common/http";


@Component({
  selector: 'app-blog-component',
  templateUrl: './blog.component.html',
  styleUrls: ['blog.component.css'],
  host: {
    class: "container-fluid d-flex p-0 m-0"
  },
  encapsulation: ViewEncapsulation.None
})

export class BlogComponent implements OnInit{

  public isCreator: boolean = false;

  public currentType: string;
  public tags: TagViewData[] = [];
  public activeTags: string[];

  private username: string;

  constructor(private jwtHelper: JwtHelperService,
              private router: Router,
              private activateRoute: ActivatedRoute,
              @Inject("BASE_URL") private baseUrl: string,
              private tokenService: TokenService,
              private serverService: ServerService
              ) {}

  ngOnInit(): void {
    this.currentType = "home";
    this.username = this.activateRoute.snapshot.paramMap.get('username');

    if (!this.username) this.router.navigateByUrl("" + this.tokenService.Username).then(() => {});//if url was blank use username in token and go to it

    this.isCreator = this.username == this.tokenService.Username;
    this.getTags();
  }

  public getTags(){
    let params = new HttpParams();
    if (this.currentType != "home")
      params = params.set("type", this.currentType);

    this.serverService.getTags(this.username, params).subscribe(response => {
        this.tags = response;
    }, error => console.log(error));
  }

  public setType(type: string){
    this.currentType = type;

    this.getTags();
  }

  public logOut() {
    this.tokenService.removeToken();
    this.router.navigateByUrl("").then(() => {} );
  }

  public tagIt(tag: TagViewData){
    tag.active = !tag.active;
    this.activeTags = this.tags.filter(t => t.active).map(t => t.name);
  }
}
