import { Routes } from '@angular/router';
import {DreamPage} from './components/dream/dream-page/dream-page';
import {DreamsList} from './components/dream/dreams-list/dreams-list';
import {Login} from './components/auth/login/login';
import {Register} from './components/auth/register/register';

export const routes: Routes = [
  { path: '', component: DreamsList },
  { path: 'dream/:dreamId', component: DreamPage },
  { path: 'login', component: Login },
  { path: 'reg', component: Register }
];
