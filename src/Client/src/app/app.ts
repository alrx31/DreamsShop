import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {DreamsList} from './dreams-list/dreams-list';
import {Header} from './header/header';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, DreamsList, Header],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected title = 'Client';
}
