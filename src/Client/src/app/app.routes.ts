import { Routes } from '@angular/router';
import {DreamPage} from './dream-page/dream-page';
import {DreamsList} from './dreams-list/dreams-list';
import {Login} from './login/login';

export const routes: Routes = [
  { path: '', component: DreamsList },
  { path: 'dream/:dreamId', component: DreamPage },
  { path: 'login', component: Login }

];
