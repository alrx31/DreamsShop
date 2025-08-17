import { Component } from '@angular/core';
import { environment } from '../../../environment/environment';
import { Router } from '@angular/router';

@Component({
  selector: 'app-logout',
  imports: [],
  templateUrl: './logout.html',
  styleUrl: './logout.scss'
})

export class Logout {
  constructor(private router: Router) {}
  
  logout() {
    localStorage.removeItem(environment.accessTokenName);
    localStorage.removeItem('UserInfo');
    
    this.router.navigate(['/login']);
  }
}
