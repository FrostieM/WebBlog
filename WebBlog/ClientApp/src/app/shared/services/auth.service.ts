import {Inject, Injectable} from "@angular/core";
import {HttpClient, HttpHeaders} from "@angular/common/http";

@Injectable()
export class AuthService{
  constructor(private http: HttpClient,
              @Inject("BASE_URL") private baseUrl: string) {}

  public login(jsonBody: string){
    return this.request(this.baseUrl + "api/auth/login", jsonBody);
  }

  public registration(jsonBody: string){
    return this.request(this.baseUrl + "api/auth/signUp", jsonBody);
  }

  private request(url: string, jsonBody: string){
    return this.http.post<any>(url, jsonBody, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    });
  }
}
