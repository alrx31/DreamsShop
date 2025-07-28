import { Component } from '@angular/core';
import {Router} from '@angular/router';

@Component({
  selector: 'app-back-button',
  imports: [],
  templateUrl: './back-button.html',
  styleUrl: './back-button.scss'
})
export class BackButton {
  constructor(private router: Router) {}

  onClick(){
    this.router.navigate(['/']);
  }
}
