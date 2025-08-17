import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../environment/environment';
import {Router} from '@angular/router';
import {UserAfterLoginInfo} from '../../environment/UserAfterLoginInfo';

interface loginResponse {
  accessToken: string;
  userData: UserAfterLoginInfo;
}

interface registerRequest {
  email: string;
  password: string;
  passwordRepeat: string;
  name: string;
}

@Injectable({
  providedIn: 'root'
})

export class Auth {
  private urlSuffix = 'ConsumerAuth';

  constructor(private http: HttpClient,
              private router: Router) { }

  refreshToken(){
    return this.http.patch<string>(`${environment.authUrl}${this.urlSuffix}`, {});
  }

  login(email: string, password: string){
    return this.http.post<loginResponse>(`${environment.authUrl}${this.urlSuffix}`, {email, password});
  }

  register(model: registerRequest){
    return this.http.put(`${environment.authUrl}${this.urlSuffix}`, model)
  }

  logout(){
    localStorage.removeItem(environment.accessTokenName);
    this.router.navigate(['/login']);
  }
}
