import { Component } from '@angular/core';
import {MatHeaderCell, MatHeaderRow} from '@angular/material/table';
import {MatExpansionPanelHeader} from '@angular/material/expansion';
import {MatTabNavPanel} from '@angular/material/tabs';
import {MatToolbar} from '@angular/material/toolbar';
import {MatSidenavContainer, MatSidenavContent} from '@angular/material/sidenav';
import {MatNavList} from '@angular/material/list';
import {MatIcon} from '@angular/material/icon';
import {RouterOutlet} from '@angular/router';
import {MatMenu, MatMenuTrigger} from '@angular/material/menu';

@Component({
  selector: 'app-header',
  imports: [
  ],
  templateUrl: './header.html',
  styleUrl: './header.scss'
})
export class Header {

}
