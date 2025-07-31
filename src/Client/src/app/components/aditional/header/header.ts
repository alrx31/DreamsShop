import { Component } from '@angular/core';
import {MatMenu, MatMenuTrigger} from '@angular/material/menu';
import { Logout } from "../logout/logout";
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-header',
  imports: [
    RouterLink,
    Logout
],
  templateUrl: './header.html',
  styleUrl: './header.scss'
})
export class Header {

}
