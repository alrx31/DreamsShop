import { Routes } from '@angular/router';
import {DreamPage} from './components/dream-page/dream-page';
import {DreamsList} from './components/dreams-list/dreams-list';
import {Login} from './components/login/login';

export const routes: Routes = [
  { path: '', component: DreamsList },
  { path: 'dream/:dreamId', component: DreamPage },
  { path: 'login', component: Login }

];
