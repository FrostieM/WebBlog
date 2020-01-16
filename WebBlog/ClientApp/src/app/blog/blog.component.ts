import {Component, Inject, OnInit, ViewEncapsulation} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {JwtHelperService} from "@auth0/angular-jwt";
import {TokenHelpers} from "../shared/services/helpers/token-helper.service";
import {TagViewData} from "../shared/classes/tagViewData.class";
import {HttpClient, HttpHeaders, HttpParams} from "@angular/common/http";


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
              private http: HttpClient,
              @Inject("BASE_URL") private baseUrl: string
              ) {}

  ngOnInit(): void {
    this.currentType = "article";
    this.username = this.activateRoute.snapshot.paramMap.get('username');

    if (!this.username) this.router.navigateByUrl("" + TokenHelpers.TOKEN_USERNAME).then(() => {});//if url was blank use username in token and go to it

    this.isCreator = this.username == TokenHelpers.TOKEN_USERNAME;
    this.getTags();
  }

  public getTags(){
    let params = new HttpParams();
    if (this.currentType != "home")
      params = params.set("type", this.currentType);

    this.http.get<TagViewData[]>(this.baseUrl + "api/postTags/" + this.username, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      }),
      params: params
    }).subscribe(response => {
        this.tags = response;
    },
        error => console.log(error));
  }

  public setType(type: string){
    this.currentType = type;

    this.getTags();
  }

  public logOut() {
    TokenHelpers.removeToken();
    this.router.navigateByUrl("").then(() => {} );
  }

  public tagIt(tag: TagViewData){
    tag.active = !tag.active;
    this.activeTags = this.tags.filter(t => t.active).map(t => t.name);
  }
}
