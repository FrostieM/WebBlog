import {Component, OnInit, ViewEncapsulation} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {TagViewData} from "../shared/classes/tagViewData.class";
import {TokenService} from "../shared/services/token.service";
import {ServerService} from "../shared/services/server.service";
import {IUser} from "../shared/interfaces/user.interface";


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

  public userInfo: IUser;

  private username: string;

  constructor(private router: Router,
              private activateRoute: ActivatedRoute,
              private tokenService: TokenService,
              private serverService: ServerService
              ) {}

  ngOnInit(): void {
    this.currentType = "home";
    this.username = this.activateRoute.snapshot.paramMap.get('username');

    //if url was blank use username in token and go to it
    if (!this.username) this.router.navigateByUrl("" + this.tokenService.Username).then(() => {});

    this.isCreator = this.username == this.tokenService.Username;
    this.getUserInfo();
    this.getTags();
  }

  public getTags(){

    this.serverService.getTags(this.username, this.currentType).subscribe(response => {
        this.tags = response;
    }, error => console.log(error));
  }

  public getUserInfo(){
    this.serverService.getUserInfo(this.username).toPromise().then(response =>{
      this.userInfo = response;
    });
  }

  public createFullSrc(fileUrl){
    return "https://localhost:5001/" + fileUrl;
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
