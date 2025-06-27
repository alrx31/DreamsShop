import { Component } from '@angular/core';
import {Router, RouterOutlet} from '@angular/router';
import {Header} from './components/aditional/header/header';
import {routes} from './app.routes';
import {environment} from './environment/environment';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})

export class App {
  protected title = 'Client';

  constructor(private router:Router) {
  }
  ngOnInit(){
    const token = localStorage.getItem(environment.accessTokenName);
    if(!token){
      this.router.navigate(['/login']);
    }
  }
}
