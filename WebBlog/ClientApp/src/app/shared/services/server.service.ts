import {Inject, Injectable} from "@angular/core";
import {HttpClient, HttpHeaders, HttpParams} from "@angular/common/http";
import {TagViewData} from "../classes/tagViewData.class";
import {Observable} from "rxjs";
import {IUserPostsViewData} from "../interfaces/userPostsViewData.interface";
import {IUser} from "../interfaces/user.interface";
import {IComment} from "../interfaces/comment.interface";
import {IPost} from "../interfaces/post.interface";
import {IInfoItem} from "../interfaces/info-item.interface";

@Injectable()
export class ServerService {
  constructor(private http: HttpClient,
              @Inject("BASE_URL") private baseUrl: string) {
  }

  public getTags(username: string, type: string): Observable<TagViewData[]>{
    let params = new HttpParams();
    if (type != "home")
      params = params.set("type", type);

    return this.getRequest<TagViewData[]>(this.baseUrl + "api/blogInfo/getTags/" + username, params);
  }

  public getUserInfo(username: string): Observable<IUser>{
    return this.getRequest<IUser>(this.baseUrl + "api/blogInfo/getUserInfo/" + username);
  }

  public LikePost(id: number): Observable<IInfoItem<IPost>>{
    let params = new HttpParams().set("postId", id.toString());
    return this.getRequest<IInfoItem<IPost>>(this.baseUrl + "api/postLike", params);
  }

  public deletePost(id: number): Observable<any>{
    let params = new HttpParams().set("postId", id.toString());
    return this.getRequest<any>(this.baseUrl + "api/posts/deletePost", params, "text" as "json");
  }

  public savePost(body: FormData): Observable<any>{
    return this.postRequest<any>(this.baseUrl + "api/posts/savePost", body, {}, "text" as "json");
  }

  public getPosts(type: string, username: string, tags: string[], currentPage: number): Observable<IUserPostsViewData>{
    return this.postRequest<IUserPostsViewData>(this.baseUrl + "api/posts/", JSON.stringify({
      type: type,
      username: username,
      tags: tags,
      currentPage: currentPage
      }),
      {"Content-Type": "application/json"});
  }

  public getComments(postId: number, commentId?: number): Observable<IInfoItem<IComment>[]>{
    let params = new HttpParams();
    if (commentId) params = params.set("parentId", commentId.toString());
    return this.getRequest<IInfoItem<IComment>[]>("api/comment/getComments/" + postId, params);
  }

  public saveComment(content: string, postId: number, commentId?: number): Observable<IInfoItem<IComment>>{
    return this.postRequest<IInfoItem<IComment>>("api/comment/saveComment", JSON.stringify({
      content: content,
      postId: postId,
      commentId: commentId
      }),
      {"Content-Type": "application/json"});
  }

  public deleteComment(id: number): Observable<any>{
    let params = new HttpParams().set("commentId", id.toString());
    return this.getRequest<any>("api/comment/deleteComment", params, "text" as "json");
  }

  public LikeComment(id: number): Observable<IInfoItem<IComment>>{
    let params = new HttpParams().set("commentId", id.toString());
    return this.getRequest<IInfoItem<IComment>>(this.baseUrl + "api/commentLike", params);
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
