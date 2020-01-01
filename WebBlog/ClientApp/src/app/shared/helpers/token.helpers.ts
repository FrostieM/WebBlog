import {JwtHelperService} from "@auth0/angular-jwt";

export abstract class TokenHelpers {
  public static get TOKEN(){
    return localStorage.getItem(this.token_name);
  }

  public static set TOKEN(token: string){
    localStorage.setItem(this.token_name, token)
  }

  public static get TOKEN_USERNAME() {
    return Object.values(this._jwt.decodeToken(this.TOKEN))[0];
  }

  public static get IS_TOKEN_CORRECT(): boolean{
    return this.TOKEN && !this._jwt.isTokenExpired(this.TOKEN);
  }

  public static removeToken(){
    localStorage.removeItem(this.token_name)
  }

  private static token_name = "token";
  private static _jwt: JwtHelperService = new JwtHelperService();
}
