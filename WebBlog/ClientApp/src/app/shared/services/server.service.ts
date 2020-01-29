import {Inject, Injectable} from "@angular/core";
import {HttpClient, HttpHeaders, HttpParams} from "@angular/common/http";
import {TagViewData} from "../classes/tagViewData.class";
import {Observable} from "rxjs";
import {IUserPostsViewData} from "../interfaces/userPostsViewData.interface";
import {ILikeViewData} from "../interfaces/likeViewData.interface";
import {IPost} from "../interfaces/post.interface";
import {IUser} from "../interfaces/user.interface";

@Injectable()
export class ServerService {
  constructor(private http: HttpClient,
              @Inject("BASE_URL") private baseUrl: string) {
  }

  public getTags(username: string, params: HttpParams): Observable<TagViewData[]>{
    return this.getRequest<TagViewData[]>(this.baseUrl + "api/blogInfo/getTags/" + username, params);
  }

  public getUserInfo(username: string): Observable<IUser>{
    return this.getRequest<IUser>(this.baseUrl + "api/blogInfo/getUserInfo/" + username);
  }

  public getPost(params: HttpParams): Observable<ILikeViewData<IPost>>{
    return this.getRequest<ILikeViewData<IPost>>(this.baseUrl + "api/postLike", params);
  }

  public deletePost(params: HttpParams): Observable<any>{
    return this.getRequest<any>(this.baseUrl + "api/posts/deletePost", params, 'text' as 'json');
  }

  public savePost(body): Observable<any>{
    return this.postRequest<any>(this.baseUrl + "api/posts/savePost", body, {}, "text" as "json");
  }

  public getPosts(body): Observable<IUserPostsViewData>{
    return this.postRequest<IUserPostsViewData>(this.baseUrl + "api/posts/", body,
      {"Content-Type": "application/json"});
  }

  private getRequest<T>(url: string, params?: HttpParams, responseType?): Observable<T>{
    return this.http.get<T>(url, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      }),
      params: params,
      responseType: responseType
    });
  }

  private postRequest<T>(url: string, body, headers?, responseType?): Observable<T>{
    return this.http.post<any>(url, body, {
      headers: new HttpHeaders(headers),
      responseType: responseType
    })
  }
}
